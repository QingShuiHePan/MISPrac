<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ShowHtCnt.aspx.cs" Inherits="Web.GroupPage.ShowHtCnt" %>
<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:Panel ID="PanelHTMLEditJS" runat="server" Visible="False">
        <script src="../Resource/CKEditor4/ckeditor.js"></script>
    
    </asp:Panel>
    <script src="../Resource/CKEditor5/Classic/ckeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Panel ID="PanelShowHTMLPage" CssClass="HTMLPageContent" runat="server" Visible="False">
        <asp:Panel ID="PanelHTMLEditControl" runat="server" Visible="False">
            
            <sspu:JTabs runat="server" >
                <TabItem TabName="编辑HTML页面内容">
                    <asp:TextBox ID="txtarea" runat="server" EnableViewState="False"  ClientIDMode="Static" TextMode="MultiLine"> </asp:TextBox>
                    
                    <br/>
                    <br/>
                    <asp:Button ID="ButtonAddContent" runat="server" Text="添加页面内容" />
                    
                    <sspu:JHighLightContent runat="server" ID="HighLight_EditPageDesignDone" ContentType="HEIGHLIGHT" Visible="False" Title="" Height="40" />

                </TabItem>
                <TabItem TabName="文件浏览">
                    <div class="PageContentsSetting ">
                        <sspu:FileManager runat="server" ID="FileManager1" EditType="DenyEdit" Width="1000" />
                    </div>

                </TabItem>
            </sspu:JTabs>
            
            
            
            <script>
                CKEDITOR.replace('txtarea');
                window.onload = function() {
                    CKEDITOR.replace( 'txtarea' );
                };
            </script>

        </asp:Panel>
        <asp:Label runat="server" ID="LabelHTMLContent"  Visible="False" CssClass="LabelHTMLContent"></asp:Label>
        <script>
            $(function () {
                
                $('.pageContent').css("height", (parseInt($('#ContentPlaceHolder1_PanelShowHTMLPage').css("height"), 10) + 500) + "px");

               


            });


        </script>
    </asp:Panel>
    
    <script>
        ClassicEditor
            .create( document.querySelector( '#ContentPlaceHolder1_JDataTable1_ctl04_Details' ) )
            .catch( error => {
                console.error( error );
            });
    </script>

</asp:Content>
