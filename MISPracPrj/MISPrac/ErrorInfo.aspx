<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorInfo.aspx.cs" Inherits="MISPrac.ErrorInfo" %>

<%@ Register Assembly="SSPUCore" Namespace="SSPUCore.Controls" TagPrefix="sspu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style type="text/css">
        html
        {
            width: 100%;
            background-image: url("/Resource/Image/404errorBG.jpg");
            background-repeat: repeat-x;
            background-color: white;
        }
        
        .errorInfo
        {
            background-image: url("/Resource/Image/404error.png");
            width: 911px;
            height: 263px;
            margin-top: 50px;
        }
        .errorCode
        {
            /*background-image: url("/Resource/Image/errorCode.png");*/
            width: 120px;
            height: 120px;
            float: right;
            margin: 50px 20px auto auto;
        }
    </style>
    <form id="form1" runat="server">
    <center>
        <div class="errorInfo">
        </div>
        <div style="width: 911px;">
            <div class="errorCode">
            </div>
        </div>
    </center>
    </form>
</body>
</html>
