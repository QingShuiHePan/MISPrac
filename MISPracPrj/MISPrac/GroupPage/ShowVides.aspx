<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ShowVides.aspx.cs" Inherits="Web.GroupPage.ShowVides" %>

<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ui-corner-all" style="width: 100%; height: 100%; background-color: black">
        <div style="height:5px;width100%;"></div>
        <sspu:JRadios_LinkUrls runat="server" CssClass="VideoTitle" ID="JRadios1"></sspu:JRadios_LinkUrls>
        <div style="height:5px;width100%;"></div>

        <center>
            <sspu:JArtPlayer runat="server" ID="JArtPlayer1" CssClass="JArtPlayerClass" Autoplay="True" AutoSize="True" Height="600" Width="1000"></sspu:JArtPlayer>
        </center>
        <div style="height:30px;width100%;"></div>
    </div>

   <style type="text/css">
       .VideoTitle fieldset 
       {
           border:none;
       }
   </style>
</asp:Content>
