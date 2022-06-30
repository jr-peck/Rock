// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

import { Guid } from "@Obsidian/Types";
import { doApiCall, HttpBodyData, HttpMethod, HttpResult, HttpUrlParams } from "@Obsidian/Utility/http";
import { Component, defineComponent, onErrorCaptured, onMounted, PropType, provide, ref } from "vue";
import { BlockHttp, InvokeBlockActionFunc } from "@Obsidian/Utility/block";
import { useStore } from "@Obsidian/PageState";
import { RockDateTime } from "@Obsidian/Utility/rockDateTime";
import { areEqual } from "@Obsidian/Utility/guid";
import { PanelAction } from "@Obsidian/Types/Controls/panelAction";
import { ObsidianBlockConfigBag } from "@Obsidian/ViewModels/CMS/obsidianBlockConfigBag";

const store = useStore();

// Can be removed once WebForms is no longer in use.
// eslint-disable-next-line @typescript-eslint/naming-convention,@typescript-eslint/no-explicit-any
declare const Sys: any;

/**
 * Handles the logic to detect when the standard block settings modal has closed
 * via a Save click for the specified block.
 * 
 * @param blockId The unique identifier of the block to be watched.
 * @param callback The callback to be invoked when the block settings have been saved.
 */
function addBlockChangedEventListener(blockId: Guid, callback: (() => void)): void {
    function onTriggerClick(): void {
        const dataElement = document.querySelector("#rock-config-trigger-data") as HTMLInputElement;
        if (dataElement.value.toLowerCase().startsWith("block_updated:")) {
            const dataSegments = dataElement.value.toLowerCase().split(":");

            if (dataSegments.length >= 3 && areEqual(dataSegments[2], blockId)) {
                callback();
            }
        }
    }

    document.querySelector("#rock-config-trigger")?.addEventListener("click", onTriggerClick, true);

    // This code can be removed once WebForms is no longer in use.
    if (Sys) {
        Sys.Application.add_load(() => {
            document.querySelector("#rock-config-trigger")?.addEventListener("click", onTriggerClick, true);
        });
    }
}

/**
 * Update the custom actions in the configuration bar to match those provided.
 * 
 * @param blockContainerElement The element that contains the block component.
 * @param actions The array of actions to put in the configuration bar.
 */
function updateConfigurationBarActions(blockContainerElement: HTMLElement, actions: PanelAction[]): void {
    // Find the configuration bar. We don't want to use querySelector at the
    // blockContent level because that would include the block content which
    // might have matching class names and cause issues.
    const blockContent = blockContainerElement.closest(".block-content");
    const blockConfiguration = Array.from(blockContent?.children ?? [])
        .find(el => el.classList.contains("block-configuration"));
    const configurationBar = blockConfiguration?.querySelector(".block-configuration-bar") as HTMLElement | undefined;

    if (!configurationBar) {
        return;
    }

    // Find the name element, which is what we will use as our insertion point.
    const nameElement = Array.from(configurationBar.children).find(el => el.tagName == "SPAN");
    if (!nameElement) {
        return;
    }

    // Find and remove any existing custom actions.
    Array.from(configurationBar.querySelectorAll("a"))
        .filter(el => el.dataset["custom-action"])
        .forEach(el => el.remove());

    // Add new custom actions.
    actions.forEach(action => {
        const hyperlinkElement = document.createElement("a");
        hyperlinkElement.href = "#";
        hyperlinkElement.title = action.title ?? "";
        hyperlinkElement.addEventListener("click", e => {
            e.preventDefault();
            if (action.handler) {
                action.handler(e);
            }
        });

        const iconElement = document.createElement("i");
        iconElement.className = action.iconCssClass ?? "fa fa-question";

        hyperlinkElement.appendChild(iconElement);

        nameElement.after(hyperlinkElement);
    });
}

export default defineComponent({
    name: "RockBlock",

    props: {
        config: {
            type: Object as PropType<ObsidianBlockConfigBag>,
            required: true
        },
        blockComponent: {
            type: Object as PropType<Component>,
            default: null
        },
        startTimeMs: {
            type: Number as PropType<number>,
            required: true
        }
    },

    setup(props) {
        const error = ref("");
        const finishTimeMs = ref<number | null>(null);
        const blockContainerElement = ref<HTMLElement | null>(null);
        const customActionComponent = ref<Component | null>(null);

        const httpCall = async <T>(method: HttpMethod, url: string, params: HttpUrlParams = undefined, data: HttpBodyData = undefined): Promise<HttpResult<T>> => {
            return await doApiCall<T>(method, url, params, data);
        };

        const get = async <T>(url: string, params: HttpUrlParams = undefined): Promise<HttpResult<T>> => {
            return await httpCall<T>("GET", url, params);
        };

        const post = async <T>(url: string, params: HttpUrlParams = undefined, data: HttpBodyData = undefined): Promise<HttpResult<T>> => {
            return await httpCall<T>("POST", url, params, data);
        };

        const invokeBlockAction: InvokeBlockActionFunc = async <T>(actionName: string, data: HttpBodyData = undefined) => {
            return await post<T>(`/api/v2/BlockActions/${store.state.pageGuid}/${props.config.blockGuid}/${actionName}`, undefined, {
                __context: {
                    pageParameters: store.state.pageParameters
                },
                ...data
            });
        };

        // Called when an error in a child component has been captured.
        onErrorCaptured(err => {
            const defaultMessage = "An unknown error was caught from the block.";

            if (err instanceof Error) {
                error.value = err.message || defaultMessage;
            }
            else if (err) {
                error.value = JSON.stringify(err) || defaultMessage;
            }
            else {
                error.value = defaultMessage;
            }
        });

        // Called when the component has mounted and is presented on the UI.
        onMounted(() => {
            finishTimeMs.value = RockDateTime.now().toMilliseconds();
            const componentName = props.blockComponent?.name || "";
            const nameParts = componentName.split(".");
            let subtitle = nameParts[0] || "";

            if (subtitle && subtitle.indexOf("(") !== 0) {
                subtitle = `(${subtitle})`;
            }

            if (nameParts.length) {
                store.addPageDebugTiming({
                    title: nameParts[1] || "<Unnamed>",
                    subtitle: subtitle,
                    startTimeMs: props.startTimeMs,
                    finishTimeMs: finishTimeMs.value
                });
            }

            // If we have any custom configuraiton actions then populate the
            // custom buttons in the configuration bar.
            if (blockContainerElement.value) {
                const customActions: PanelAction[] = [];

                if (props.config.customConfigurationActions) {
                    for (const cca of props.config.customConfigurationActions) {
                        if (cca.iconCssClass && cca.tooltip && cca.componentFileUrl) {
                            customActions.push({
                                type: "default",
                                title: cca.tooltip,
                                iconCssClass: cca.iconCssClass,
                                handler: async () => {
                                    try {
                                        const module = await import(cca.componentFileUrl ?? "");
                                        customActionComponent.value = module?.default ?? module ?? null;
                                    }
                                    catch (e) {
                                        // Log the error, but continue setting up the app so the UI will show the user an error
                                        console.error(e);
                                    }
                                }
                            });
                        }
                    }
                }

                updateConfigurationBarActions(blockContainerElement.value, customActions);
            }
        });

        const onCustomActionClose = (): void => {
            console.log("clear");
            customActionComponent.value = null;
        };

        const blockHttp: BlockHttp = { get, post };

        provide("http", blockHttp);
        provide("invokeBlockAction", invokeBlockAction);
        provide("configurationValues", props.config.configurationValues);

        if (props.config.blockGuid) {
            addBlockChangedEventListener(props.config.blockGuid, () => console.log(`block ${props.config.blockGuid} updated`));
        }

        return {
            blockContainerElement,
            blockFileUrl: props.config.blockFileUrl,
            customActionComponent,
            onCustomActionClose,
            error
        };
    },

    // Note: We are using a custom alert so there is no dependency on the
    // Controls package.
    template: `
<div ref="blockContainerElement" class="obsidian-block">
    <div v-if="!blockComponent" class="alert alert-danger">
        <strong>Not Found</strong>
        Could not find block component: "{{blockFileUrl}}"
    </div>

    <div v-if="error" class="alert alert-danger">
        <strong>Uncaught Error</strong>
        {{error}}
    </div>

    <component :is="blockComponent" />

    <div style="display: none;">
        <component :is="customActionComponent" @close="onCustomActionClose" />
    </div>
</div>`
});
