<%@ Page Title="" Language="C#" MasterPageFile="~/ConfigByUI/MustLogin.Master" AutoEventWireup="true" CodeBehind="ConfigUserWithGroup.aspx.cs" Inherits="MISPrac.ConfigUserWithGroup" %>

<%@ Register Assembly="SSPUCore" Namespace="SSPUCore.Controls" TagPrefix="sspu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <sspu:JDataTable ID="JDataTable1" runat="server" EditType="FullEdit"/>
</asp:Content>
