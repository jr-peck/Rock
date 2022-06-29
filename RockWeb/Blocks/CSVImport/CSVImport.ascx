<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CSVImport.ascx.cs" Inherits="RockWeb.Blocks.CVSImport.CSVImport" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
            <div class="panel-heading">
                <h1 class="panel-title pull-left">
                    <asp:Literal ID="lHeading" runat="server" Text="CSV Import" />
                </h1>
            </div>

            <div class="panel-body">
                <h2>Comma Separated File Import </h2>
                The first step is to upload your comma delimited file. We’ll then allow you to map the columns to fields in Rock. The first row of your file must contain headers for each column.

                  <hr>
                <div class="row">
                    <div class="col-lg-7">
                        <div class="row">
                            <div class="col-lg-10">
                                <Rock:RockDropDownList
                                    ID="ddlDataType"
                                    runat="server"
                                    Label="Data Type" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-5">
                                <Rock:RockTextBox
                                    ID="tbSourceDescription"
                                    runat="server"
                                    Label="Source Description"
                                    Required="true"
                                    Help="Describe where this data came from. We’ll store what you type as a setting so if you import data from this system again we’ll be able to match people you’ve already imported." />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <Rock:FileUploader
                                    ID="fupCSVFile"
                                    runat="server"
                                    OnFileUploaded="fupCSVFile_FileUploaded"
                                    Label="CSV File" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockCheckBox
                                    ID="cbAllowUpdatingExisting"
                                    runat="server"
                                    Label="Allow Updating Existing Matched Records"
                                    Checked="true"
                                    Help="When checked existing records that exist in the database that match date being imported (first name, last name and email match) will be updated. Otherwise only new records will be added to the database." />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <Rock:BootstrapButton ID="btnStart" runat="server" CssClass="btn btn-primary" Text="Start" />
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:RockRadioButtonList
                                    ID="rblpreviousSourceDescription"
                                    runat="server"
                                    Label="Previous Source Descriptions"
                                    RepeatDirection="Horizontal"
                                    Help="If you are importing data from a source that has already been run once, select the matching description below. Otherwise, if this is a different source than those shown, chose to add a new source description.">
                                    <asp:ListItem>pco</asp:ListItem>
                                    <asp:ListItem>ccb</asp:ListItem>
                                    <asp:ListItem>other</asp:ListItem>
                                </Rock:RockRadioButtonList>
                                <Rock:RockTextBox ID="tbpreviousSourceDescription" runat="server"  Style="display:none" />
                                <br />
                                <a id="add-source-description" href="javascript:void(0)">Add Additional Source Description</a>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div id="description">
                            <h4>Required Fields </h4>
                            When uploading data about people you’ll need to include the following information. These should be separate columns on your CSV file. The name of the field doesn’t matter as you’ll be able to map it later.
                            <br />
                            <br />
                            <ol>
                                <li>Id - Some form of unique identifier for the person. This should come from your former system.</li>
                                <li>Family Id - This field should be an unique value for each family. This tells us who is in the same family.</li>
                                <li>Family Role - This column should have the values of Adult or Child.</li>
                                <li>First Name - The individual’s first name.</li>
                                <li>Last Name - The individual’s last name.</li>
                            </ol>
                            <br />

                            <h4>Optional Fields</h4>
                            You may optionally add the following fields.
                            <br />
                            <br />
                            <ol>
                                <li>Nick Name - The individual’s nick name.</li>
                                <li>Middle Name - The individual’s middle name.</li>
                                <li>Suffix - The person’s suffix.
                                    <br />
                                    (______)</li>
                                <li>Home Phone - The individual’s home phone number.
                                    <br />
                                    (480-555-1234)</li>
                                <li>Mobile Phone - The individuals’s mobile phone number.
                                    <br />
                                    (480-555-1234)</li>
                                <li>Is SMS Enabled - Whether the individual allows SMS messages to tbe sent to them.</li>
                                <li>Email - The individual’s email address.</li>
                                <li>Email Preference - The permissions you have been granted to email the individual. 
                                    <br />
                                    (EmailAllowed, _____)</li>
                                <li>Gender - The gender of the individual.
                                    <br />
                                    (Male, Female)</li>
                                <li>Marital Status - The marital status of the individual.
                                    <br />
                                    (Married, Single)</li>
                                <li>Birthdate - The individual’s birthdate. </li>
                                <li>Anniversary Date - The marriage anniversary date of the individual.</li>
                                <li>Record Status - Whether the person is active or not.
                                    <br />
                                    (Active, Inactive)</li>
                                <li>Inactive Reason - The reason why the person is inactive.</li>
                                <li>Is Deceased - Determines whether the person is deceased.
                                    <br />
                                    (True,False)</li>
                                <li>Connection Status - The connection type the individual has to your organization. This must match the connection statuses you have in Rock.</li>
                                <li>Grade - The grade of the individual.
                                    <br />
                                    (______)</li>
                                <li>Home Address Street 1 - The first line of their home street address.</li>
                                <li>Home Address Street 2 - The second line of their home street address.</li>
                                <li>Home Address City - The city of their home address.</li>
                                <li>Home Address State - The state of their home address.
                                    <br />
                                    (CA, AZ, etc.)</li>
                                <li>Home Address Postal Code - The postal code of their home address.</li>
                                <li>Home Address Country -  The country of their home address.
                                    <br />
                                    (US)</li>
                                <li>Created Date Time - The date and time the record was originally created.
                                    <br />
                                    (1/1/2020 9:12:34 AM)</li>
                                <li>Modified Date Time - The date and time the record was last modified.
                                    <br />
                                    (5/1/2020 9:12:34 AM)</li>
                                <li>Note - A note that you would like to add to the person.</li>
                                <li>Campus Id - The Rock campus id for the individual.</li>
                                <li>Campus Name - The Rock campus name for the individual. This will update if it does not exist.</li>
                                <li>Give Individually - Determines if the person gives with the family or as an individual.
                                    <br />
                                    (True/False) </li>
                            </ol>
                            <br />
                            <br />
                            <h4>Additional Fields</h4>
                            You can provide as many other additional fields as you’d like. We’ll allow you to match these to attributes in Rock. You’ll want to make sure that these Rock attributes exist.
                            <br />
                            You can also choose to ignore columns on your CSV file.
                        </div>
                    </div>
                    <div class="col-md-6" />
                    <%-- Pad some extra space to the right of the panel after the description text ---%>
                </div>
            </div>

            <script>
                $('#add-source-description')
                    .off('click')
                    .on('click', () => {
                        $("#<%=(rblpreviousSourceDescription.ClientID)%>").hide()
                        $("#<%=(tbpreviousSourceDescription.ClientID)%>").show()
                    });
            </script>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

CSS:
----

#description {
    background-color: coral;
}