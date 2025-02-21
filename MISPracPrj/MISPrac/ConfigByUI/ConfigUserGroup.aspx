<%@ Page Title="" Language="C#" MasterPageFile="~/ConfigByUI/MustLogin.Master" AutoEventWireup="true" CodeBehind="ConfigUserGroup.aspx.cs" Inherits="MISPrac.ConfigUserGroup" %>
<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <sspu:JDataTable ID="JDataTable1" runat="server" EditType="FullEdit" />
</asp:Content>
