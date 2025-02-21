<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ShowTables.aspx.cs" Inherits="MISPrac.GroupPage.ShowTables" ValidateRequest="false" %>

<%@ Register Assembly="SSPUCore" Namespace="SSPUCore.Controls" TagPrefix="sspu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:Panel ID="PanelHTMLEditJS" runat="server" Visible="False">
        <script src="../Resource/CKEditor4/ckeditor.js"></script>
        
    </asp:Panel>
    <script src="../Resource/CKEditor5/Classic/ckeditor.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <sspu:JDataTable ID="JDataTable1" runat="server" />
    
    <asp:Panel ID="PanelShowHTMLPage" CssClass="HTMLPageContent" runat="server" Visible="False">
        <asp:Panel ID="PanelHTMLEditControl" runat="server" Visible="False">
            
            <sspu:Uploadify ID="Uploadify1" runat="server" />
            
            <asp:Button ID="ButtonAddContent" runat="server" Text="添加页面内容" />
            <asp:TextBox ID="txtarea" runat="server" EnableViewState="False"  ClientIDMode="Static" TextMode="MultiLine"> </asp:TextBox>
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
