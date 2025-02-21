<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="MISPrac.ChangePassword" %>
<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Label ID="Label_Info" runat="server" Visible="false" Text="修改密码成功！" ></asp:Label>
    <br />
    <div id="checkClientInputArea" >
      
        <div class="_OneDynamicCloumnInputRow">
            <div class="__Label__">原密码:</div>
            <sspu:JTextBox ID="JTextBox_OldPassword"            runat="server" Required="true" TextType="password" />
        
        </div>
        
        <div class="_OneDynamicCloumnInputRow">
            <div class="__Label__">新密码:</div>
            <sspu:JTextBox ID="JTextBox_NewPassword1"            runat="server" Required="true" TextType="password" />
        </div>
        
        <div class="_OneDynamicCloumnInputRow">
            <div class="__Label__">密码确认:</div>
            <sspu:JTextBox ID="JTextBox_NewPassword2"            runat="server" Required="true" TextType="password"/>
        </div>

    </div>
    <sspu:ValidatedSubmit ID="ValidatedSubmit1" runat="server" CheckedDivClientID="checkClientInputArea"  Text="确定" />
    
    <sspu:ScriptManager ID="ScriptManager1" runat="server" />

</asp:Content>
