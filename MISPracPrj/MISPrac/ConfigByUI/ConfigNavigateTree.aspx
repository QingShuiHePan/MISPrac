<%@ Page Title="" Language="C#" MasterPageFile="~/ConfigByUI/MustLogin.Master" AutoEventWireup="true" CodeBehind="ConfigNavigateTree.aspx.cs" Inherits="MISPrac.ConfigNavigateTree" %>

<%@ Import Namespace="Definition" %>

<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; float: left;">


        <%-- <div style="margin-left: 0px; height: 600px; background-color: antiquewhite;float:left;">
                
            </div>--%>

        <div class="sspu_Left">
            <div style="float: left; margin-left: 3px;">
                <sspu:Accordion runat="server" Width="200">
                    <TabItem TabName="全局导航树">
                        <div class="TableItemConfig" style="height: 930px;">
                            <sspu:JTreeViewFancy ID="JTreeView_NavDef" runat="server" EditType="FullEdit" AutoCollapse="True" ExpandAll="False"></sspu:JTreeViewFancy>
                        </div>
                    </TabItem>

                </sspu:Accordion>
            </div>

            <div style="float: left; padding-left: 3px;">
                <sspu:Accordion ID="AccodTabType" runat="server" Width="200" Height="500">


                    <TabItem TabName="显示数据表">
                        <div class="TableItemConfig">
                            <sspu:JTreeViewFancy runat="server" ID="JTreeView_TableName" ThemeType="Win8" Height="600"></sspu:JTreeViewFancy>
                        </div>

                    </TabItem>
                    <TabItem TabName="显示为页面">
                        <div class="TableItemConfig">
                            <sspu:JTreeViewFancy runat="server" ID="JTreeView_SetNode2HTMLPage" ThemeType="Win8" Height="250"></sspu:JTreeViewFancy>
                        </div>

                    </TabItem>

                </sspu:Accordion>
            </div>

            <div style="float: left; padding-left: 3px;">
                <sspu:Accordion runat="server" Width="200" ID="AccordionUserGrops">

                    <TabItem TabName="全部用户组">
                        <div class="TableItemConfig">
                            <sspu:JTreeViewFancy runat="server" ID="JTreeView_UserGroup"></sspu:JTreeViewFancy>
                        </div>
                    </TabItem>
                    <TabItem TabName="已定义权限" Visible="False">
                        <div class="TableItemConfig">
                            <sspu:JTreeViewFancy runat="server" ID="JTreeView_UserGroupDefined"></sspu:JTreeViewFancy>
                        </div>
                    </TabItem>
                </sspu:Accordion>
            </div>


        </div>
        <div class="ConfigPageTypeContent" style="float: left; margin-left: 610px; padding-left: 10px; padding-top: 5px;">


            <sspu:JTabs runat="server">
                <TabItem TabName="页面设置">
                    <div class="PageContentsSetting">
                        <asp:Panel runat="server" ID="Panel_SetHTMLContent" Visible="False" CssClass="SetPageTypeContent">
                            <sspu:JRadios runat="server" ID="RadioButton_HtmlPageEditType" Text="" LabelText="设置页面权限"></sspu:JRadios>
                            <sspu:JButton runat="server" Text="提交" OnClick="HtmlSetButtonOnClick" ButtonType="submit" />
                            <sspu:JHighLightContent runat="server" ID="HighLight_EditHTMLTypeDone" ContentType="HEIGHLIGHT" Visible="False" Title="" Height="40" />


                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel_SetDesignedPage" Visible="False" CssClass="SetPageTypeContent">
                            <div class="DivJDataTable_DocumentPageInfo" >
                                <sspu:JDataTable runat="server" ID="JDataTable_SetDesignedPage" EditType="FullEdit" UseSimplestStyle="True" ShowPrint="False" />
                            </div>
                            <%--<div id="checkId1">
                                <div class="_OneDynamicCloumnInputRow">
                                    <div class="__Label__">页面链接地址:</div>
                                    <sspu:JTextBox ID="JTextBox_DesignedPageUrl" runat="server" Required="true" ErrorMessage="地址不能为空" Width="400" Height="20" />
                                </div>
                            </div>
                            <sspu:JRadios runat="server" ID="JRadio_DesignPagePermision" LabelText="设置权限" CssClass="JRadio_Items" />
                            <sspu:ValidatedSubmit ID="ValidatedSubmit1" runat="server" CheckedDivClientID="checkId1" Text="提交" OnClick="ValidatedSubmit1_OnSetDesignPage" />
                            <sspu:JHighLightContent runat="server" ID="HighLight_EditPageDesignDone" ContentType="HEIGHLIGHT" Visible="False" Title="" Height="40" />--%>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel_SetDocumentPage" Visible="True">

                            <div class="DivJDataTable_DocumentPageInfo" >
                                <sspu:JDataTable ID="JDataTable_DocumentPageInfo" runat="server" EditType="FullEdit" ShowPrint="False" AutoWidth="False" EnableScrollX="True" 
                                    LengthChange="True" UseSimplestStyle="True" />
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel_SetVideoPage" Visible="False">
                            <div class="DivJDataTable_DocumentPageInfo" >
                                
                                <sspu:JDataTable ID="JDataTable_VedioPageInfo" runat="server" EditType="FullEdit" ShowPrint="False" AutoWidth="False" EnableScrollX="True" 
                                                 LengthChange="True" UseSimplestStyle="True" />
                                
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="Panel_SetDataTable" Visible="False" CssClass="">
                            
                            <div class="DivJDataTable_DocumentPageInfo" style="width: 100%;">
                                <div class=" ui-corner-all" style="font-size: 12pt; text-align: left;height:25px;  vertical-align:central;">
                                    设置数据表权限
                                </div>
                                <asp:Panel runat="server" ID="PanelDataFilter" CssClass="TableNamesList">
  
                                    <div class="content">
                                        <div class="SetPageTypeContent box ui-corner-all" style="width:auto;height:auto;" >
                                            <asp:Label runat="server" ID="LabelTableColumns"></asp:Label>
                                            <br/>
                                            <br/>
                                            <span>数据筛选条件：</span> <sspu:JTextBox runat="server" ID="DataFilter" Width="500" Height="25"  />
                                            <br/>
                                            <br/>
                                            <sspu:JRadios runat="server" ID="JRadios_SetTableAuthorPermit" LabelText="设置权限" CssClass="JRadio_Items" />
                                            <br/>
                                            <br/>
                                            <sspu:JButton runat="server" ButtonType="submit" Text="提交" OnClick="AddDataFilter" />
                                            <sspu:JHighLightContent runat="server" ID="JHighLightContent_EditTabAuthor" ContentType="HEIGHLIGHT" Visible="False" Title="" Height="40" />
                                        </div>
                                    </div>

                                </asp:Panel>
                                <br />
                                <%--<sspu:JDataTable ID="JDataTableTable_SetTableAutor" runat="server" EditType="FullEdit" UseSimplestStyle="False" ShowPrint="False" />--%>
                                
                                <div class=" ui-corner-all" style="font-size: 12pt; text-align: left ;margin-top:40px;height:30px;padding-top:10px;">
                                    设置字段权限
                                </div>
                                <br />
                                <div class="DivJDataTable_DocumentPageInfo">
                                    <sspu:JDataTable ID="JDataTable_SetTablColumnAuthority" runat="server" EditType="FullEdit" UseSimplestStyle="True" ShowPrint="False" />
                                </div>
                                
                                
                            </div>
                        </asp:Panel>
                    </div>

                </TabItem>
                <TabItem TabName="文件浏览">
                    <div class="PageContentsSetting ">
                        <sspu:FileManager runat="server" ID="FileManager1" EditType="DenyEdit" Width="1000" />
                    </div>

                </TabItem>
            </sspu:JTabs>

        </div>

    </div>
    
    <style type="text/css">
        .DivJDataTable_DocumentPageInfo
        {
            min-width:1000px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            // Function to set the width of the div
            function adjustDivWidth() {
                // Calculate new width
                var newWidth = $(window).width() - 850;
                // Set the width of the div
                $('.PageContentsSetting').css('width', newWidth);

                //$('.DivJDataTable_DocumentPageInfo').css('width', newWidth);
                //$('.__SSPUTable_ dataTable').css('width', newWidth);
            }

            // Adjust the div width on page load
            adjustDivWidth();

            // Adjust the div width when the window is resized
            $(window).resize(function () {
                adjustDivWidth();
            });
        });
    </script>
</asp:Content>
