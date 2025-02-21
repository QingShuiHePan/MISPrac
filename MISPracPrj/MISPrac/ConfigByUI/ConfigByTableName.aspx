<%@ Page Title="" Language="C#" MasterPageFile="~/ConfigByUI/MustLogin.Master" AutoEventWireup="true" CodeBehind="ConfigByTableName.aspx.cs" Inherits="MISPrac.ConfigByTableName" %>
<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; float: left;">
           
            
            <div class="sspu_Left" width="606px">
                    <div style="float: left">
                        <sspu:Accordion runat="server" Width="200">
                            <TabItem TabName="显示数据表">
                                <div class="TableItemConfig">
                                    <sspu:JTreeViewFancy runat="server" ID="JTreeView_TableName" ThemeType="Win8"></sspu:JTreeViewFancy>
                                </div>
                            </TabItem>
                        </sspu:Accordion>
                        
                    </div>

                    <div style="float: left; padding-left: 3px;">
                        <sspu:Accordion runat="server" Width="200">
                            <TabItem TabName="全局导航树">
                                <div class="TableItemConfig">
                                    <sspu:JTreeViewFancy ID="JTreeView_NavDef" runat="server" EditType="FullEdit"></sspu:JTreeViewFancy>
                                </div>
                            </TabItem>

                        </sspu:Accordion>
                    </div>

                    <div style="float: left; padding-left: 3px; ">
                        <sspu:Accordion runat="server" Width="200">
                            <TabItem TabName="权限定义">
                                <div class="TableItemConfig">
                                    <sspu:JTreeViewFancy runat="server" ID="JTreeView_UserGroup"></sspu:JTreeViewFancy>
                                </div>
                            </TabItem>
                        </sspu:Accordion>
                    </div>
                    
                    
                </div>
                
                <div id="ConfigNavTableContainer" style="float: left; margin-left: 606px">
                    <br/>
                  
                    <div class="" >
                        <div class=" ui-state-highlight ui-corner-all" style="font-size: 12pt;text-align: center">
                            设置数据表权限
                        </div>
                        <br/>
                        <sspu:JDataTable ID="JDataTableTable" runat="server"  EditType="FullEdit" UseSimplestStyle="False" ShowPrint="False" />
                    </div>
                    <br/>
                    <br/>
                    <div class=" " >
                        <div class=" ui-state-highlight ui-corner-all" style="font-size: 12pt;text-align: center">
                            设置字段权限
                        </div>
                        <br/>
                        <sspu:JDataTable ID="JDataTable1" runat="server"  EditType="FullEdit" UseSimplestStyle="True" ShowPrint="False" />
                    </div>

                        
                </div>

        </div>
</asp:Content>
