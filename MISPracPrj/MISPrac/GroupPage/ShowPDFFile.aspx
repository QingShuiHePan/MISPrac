<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ShowPDFFile.aspx.cs" Inherits="Web.GroupPage.ShowPDFFile" %>
<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Resource/CSS/PDF/kendo.default-v2.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../Resource/JS/PDF/kendo.all.min.js"></script>
    <script type="text/javascript" src="../Resource/JS/PDF/pdf.js"></script>
    <script type="text/javascript">
        window.pdfjsLib.GlobalWorkerOptions.workerSrc = "../Resource/JS/PDF/pdf.worker.js";
    </script>
    <div id="example">
        <sspu:JRadios_LinkUrls runat="server" ID="JRadios1" CssClass="VideoTitle"></sspu:JRadios_LinkUrls>
        <div id="pdfViewer" >
        </div>
    </div>
    <style type="text/css">
        .VideoTitle fieldset {
            border: none;
        }
    </style>
    <script>
        var firstRender = true;
        $(document).ready(function () {
            var numeric = $("#numeric").kendoNumericTextBox({
                change: onChange,
                spin: onSpin,
                format: "n0",
                value: 1
            }).data("kendoNumericTextBox");

            var pdfViewer = $("#pdfViewer").kendoPDFViewer({
                pdfjsProcessing: {
                    file: "<%= PDFFilePath %>"
                },
                width: "100%",
                
                render: function (e) {
                    if (firstRender) {
                        e.sender.toolbar.zoom.combobox.value("fitToWidth");
                        e.sender.toolbar.zoom.combobox.trigger("change");
                        firstRender = false;
                    }
                }
            }).getKendoPDFViewer(); 

            $("#loadFile").click(function () {
                //pdfViewer.fromFile("/Resource/File/test.pdf");
            });

            function onChange(e) {
                var value = this.value();
                changePdfViewerPage(value)
            }

            function onSpin(e) {
                var value = this.value();
                changePdfViewerPage(value)
            }

            function changePdfViewerPage(value) {
                pdfViewer.activatePage(value);
            }
        });
    
    </script>
</asp:Content>
