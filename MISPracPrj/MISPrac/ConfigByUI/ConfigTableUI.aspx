<%@ Page Title="" Language="C#" MasterPageFile="~/ConfigByUI/MustLogin.Master" AutoEventWireup="true" CodeBehind="ConfigTableUI.aspx.cs" Inherits="MISPrac.ConfigTableUI" %>

<%@ Register TagPrefix="sspu" Namespace="SSPUCore.Controls" Assembly="SSPUCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        input {
            height: 1.8em;
        }

        .CheckBoxControl {
            width: 1.5em;
            height: 1.5em;
        }

        .ButtonSubmit {
            height: 2.5em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; float: left;">


        <div class="sspu_Left" width="606px">
            <div style="float: left">
                <sspu:Accordion runat="server" Width="200" AutoHeight="True">
                    <TabItem TabName="显示数据表">
                        <div class="TableItemConfig" style="height: 930px;">
                            <sspu:JTreeViewFancy runat="server" ID="JTreeView_TableName" ThemeType="Win8"></sspu:JTreeViewFancy>
                        </div>
                    </TabItem>
                </sspu:Accordion>

            </div>

            <div style="float: left; padding-left: 3px;">
                <sspu:Accordion runat="server" Width="200">
                    <TabItem TabName="表字段">
                        <div class="TableItemConfig" style="height: 930px;">
                            <sspu:JTreeViewFancy ID="JTreeView_Columns" runat="server" EditType="FullEdit"></sspu:JTreeViewFancy>
                        </div>
                    </TabItem>

                </sspu:Accordion>
            </div>

        </div>

        <asp:Panel runat="server" ID="Panel_ConfigNavTableContainer" Visible="False">
            <div id="ConfigNavTableContainer" style="float: left; margin-left: 406px">
                <br />
                <div class="ContentBoxColumn">
                    <div>必填：</div>
                    <div>
                        <sspu:JCheckBox runat="server" NeedClientData="True" RemainCheckState="True" ID="JCheckBoxRequired" CssClass="CheckBoxControl" />
                    </div>
                    <div style="margin-left: 10px;">错误信息提示：</div>
                    <div>
                        <sspu:JTextBox runat="server" Width="300" ID="JTextBox_ErrorInfo" />
                    </div>
                </div>

                <div>选择字段控件：</div>

                <div>

                    <sspu:JTabs runat="server" ID="JTabControlSelector">
                        <TabItem TabName="普通文本控件" ID="TabItemTextBox">
                            <ItemContent>
                                <sspu:JTabs ID="JTabs_TextType" runat="server">
                                    <tabitem tabname="普通文本">
                                            <ItemContent>
                                                <sspu:JButton runat="server" ID="JButton_UseTextBoxNormal" Text="确定" CssClass="ButtonSubmit"  OnClick="JButtonClicked" />
                                            </ItemContent>
                                        </tabitem>
                                    <tabitem tabname="数字类型">
                                            <ItemContent>
                                                
                                                  
                                                    <div class="box" >
                                                        
                                                        <div class="ContentBoxColumn">
                                                            <div class=" ContentBoxColumn">
                                                                <div>类型：</div> 
                                                                <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumberType">
                                                                    <Item Text="Double"></Item>
                                                                    <Item Text="Int"></Item>
                                                                </sspu:JDropDown></div>
                                                            </div>
                                                            <div>最小数字：</div> <sspu:JTextBox runat="server" Width="150" ID="JTextBox_Min" TextType="DOUBLE" Text=""/>
                                                            <div>最大数字：</div> <sspu:JTextBox runat="server" Width="150" ID="JTextBox_Max" TextType="DOUBLE" Text=""/>
                                                        </div>
                                                        
                                                    </div>
                                                    <asp:Panel runat="server" ID="Panel_JTabForTextboxNum">
                                                        <sspu:JTabs runat="server" ID="JTabs_NumberTextFilter" >
                                                            <TabItem TabName="无关联更新">
                                                                <ItemContent>
                                                                    <sspu:JButton runat="server" ID="JButton_UseTextBoxNumberNormal"  Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                                                                </ItemContent>
                                                            </TabItem>
                                                            <TabItem TabName="表内关联更新">
                                                                <ItemContent>
                                                                    <div class="ContentBoxColumn">
                                                                        <div>当前字段=</div>
                                                                        <div>(</div>
                                                                        <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumTableInnerAssociateColumn1"></sspu:JDropDown></div>
                                                                        <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumTableInnerAssociateCalculateType"></sspu:JDropDown></div>
                                                                        <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumTableInnerAssociateColumn2"></sspu:JDropDown></div>
                                                                        <div>)</div>
                                                                        <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumTableInnerAssociateCalculateType2"></sspu:JDropDown></div>
                                                                        <div><sspu:JDropDown runat="server" ID="JDropDown_TextBoxNumTableInnerAssociateColumn3"></sspu:JDropDown></div>
                                                                    </div>
                                                                    <div class="ContentBoxColumn">
                                                                          <div>关联更新表：</div><sspu:JDropDown runat="server" ID="JDropDown_InnerAssociateResultWriteOut_TableName" AutoPostBack="True" ViewStateMode="Enabled"  OnSelectedIndexChanged="JDropDown__InnerAssociateResultWriteOut_AsociatChangeTable_OnSelectedIndexChanged"  ></sspu:JDropDown>
                                                                          <div>关联更新列：</div><sspu:JDropDown runat="server" ID="JDropDown_InnerAssociateResultWriteOut_ColumnName"></sspu:JDropDown>
                                                                      </div>
                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn" ID="Panel_InnerAssociateResultWriteOut_AssociateTable1" Visible="False">
                                                                          <div>关联条件：</div>
                                                                          <sspu:JDropDown runat="server" ID="JDropDown_InnerAssociateResultWriteOut_ForeinConditionColum"></sspu:JDropDown>
                                                                          <div>=当前表的：</div>
                                                                          <div><sspu:JDropDown runat="server" ID="JDropDown_InnerAssociateResultWriteOut_CurrentConditionColum"></sspu:JDropDown></div>
                                                                      </asp:Panel>

                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn"  ID="Panel_InnerAssociateResultWriteOut_AssociatTableRefresh2" Visible="False">
                                                                          <div>
                                                                              关联系数：
                                                                          </div>
                                                                          <div>
                                                                              <sspu:JTextBox runat="server" ID="JTextbox_InnerAssociateResultWriteOut_Rate" />
                                                                          </div>
                                                                      </asp:Panel>
                                                                <sspu:JButton runat="server" ID="JButton_TextBoxNumTableInnerAssociate"  Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                                                                </ItemContent>
                                                            </TabItem>
                                                            <TabItem TabName="表外关联更新" >
                                                                <ItemContent>
                                                                      <div class="ContentBoxColumn">
                                                                          <div>关联更新表：</div><sspu:JDropDown runat="server" ID="JDropDown_AsociatChangeTable" AutoPostBack="True" ViewStateMode="Enabled"  OnSelectedIndexChanged="JDropDown_AsociatChangeTable_OnSelectedIndexChanged"  ></sspu:JDropDown>
                                                                          <div>关联更新列：</div><sspu:JDropDown runat="server" ID="JDropDown_AsociatChangeColumn"></sspu:JDropDown>
                                                                      </div>
                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn" ID="Panel_AssociatTableRefresh1" Visible="False">
                                                                          <div>关联条件：</div>
                                                                          <sspu:JDropDown runat="server" ID="JDropDown_AsociatChangeTableForeinID"></sspu:JDropDown>
                                                                          <div>=当前表的：</div>
                                                                          <div><sspu:JDropDown runat="server" ID="JDrown_CurrentAssociateColumn"></sspu:JDropDown></div>
                                                                      </asp:Panel>

                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn"  ID="Panel_AssociatTableRefresh2" Visible="False">
                                                                          <div>
                                                                              关联系数：
                                                                          </div>
                                                                          <div>
                                                                              <sspu:JTextBox runat="server" ID="JTextbox_AssociatParm" />
                                                                          </div>
                                                                      </asp:Panel>
                                                                    <sspu:JButton runat="server" ID="JButton_TextNumbAssociatTableModify"  Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                                                                </ItemContent>
                                                            </TabItem>
                                                            <TabItem TabName="读取表外数据" >
                                                                <ItemContent>
                                                                      <div class="ContentBoxColumn">
                                                                          <div>当前字段=</div>
                                                                          <div>数据源表：</div><sspu:JDropDown runat="server" ID="JDropDown_ReadAssociateOutTableName" AutoPostBack="True" ViewStateMode="Enabled"  OnSelectedIndexChanged="JDropDown_ReadAssociateOutTableName_OnSelectedIndexChanged"  ></sspu:JDropDown>
                                                                          <div>数据源列：</div><sspu:JDropDown runat="server" ID="JDropDown_ReadAssociateOutTableColumn"></sspu:JDropDown>
                                                                      </div>
                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn" ID="Panel_ReadAssociateOutTableCondition" Visible="False">
                                                                          <div>关联条件：</div>
                                                                          <sspu:JDropDown runat="server" ID="JDropDown_ReadAssociateOutTableConditionOutColumn"></sspu:JDropDown>
                                                                          <div>=当前表的：</div>
                                                                          <div><sspu:JDropDown runat="server" ID="JDropDown_ReadAssociateOutTableConditionInnerColumn"></sspu:JDropDown></div>
                                                                      </asp:Panel>
                                                                      <asp:Panel runat="server" CssClass="ContentBoxColumn"  ID="Panel_ReadAssociateOutTableCondition2" Visible="False">
                                                                          <div>关联系数：</div>
                                                                          <div><sspu:JTextBox runat="server" ID="JTextBox_ReadAssociateOutTableRate" /></div>
                                                                      </asp:Panel>
                                                                    <sspu:JButton runat="server" ID="JButton_ReadAssociateOutTable"  Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                                                                </ItemContent>
                                                            </TabItem>
                                                        </sspu:JTabs>
                                                    </asp:Panel>
                                            </ItemContent>
                                        </tabitem>
                                    <tabitem tabname="正则表达式型">
                                            <ItemContent>
                                                <div class="ContentBoxColumn">
                                                    <div>正则表达式：</div> <sspu:JTextBox runat="server"  Width="300" ID="JTextBox_Regex" Text=""/>
                                                </div>
                                                <sspu:JButton runat="server" ID="JButton_UseTextBoxRegex" Text="确定" CssClass="ButtonSubmit"  OnClick="JButtonClicked" />
                                            </ItemContent>
                                        </tabitem>
                                </sspu:JTabs>
                            </ItemContent>
                        </TabItem>
                        <TabItem TabName="菜单型控件">
                            <ItemContent>
                                <div class="ContentBoxRow">
                                    <div class="box">
                                        <div>输入菜单项，以逗号分割：</div>
                                        <sspu:JTextBox NeedClientData="True" RemainText="True" runat="server" Width="300" ID="JTextBox_DropdownItems" Text=""></sspu:JTextBox>
                                    </div>
                                </div>
                                <sspu:JButton runat="server" ID="JButton_UseDropdown" Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                            </ItemContent>
                        </TabItem>

                        <TabItem TabName="文件类型" ID="TabItemUploadFile">
                            <ItemContent>
                                <div class="ContentBoxColumn">
                                    <div>
                                        文件说明形式如:jpg,jpeg,gif,png
                                    </div>
                                    <div>
                                        文件后缀形式如：*.jpg;*.jpeg;*.gif;*.png
                                    </div>
                                </div>
                                <div class="ContentBoxColumn">
                                    <div>文件说明：</div>
                                    <sspu:JTextBox runat="server" Width="300" ID="JTextBox_FileTypesDescription" Text="" />
                                </div>
                                <div class="ContentBoxColumn">
                                    <div>文件后缀：</div>
                                    <sspu:JTextBox runat="server" Width="300" ID="JTextBox_AllowFileTypes" Text="" />
                                </div>
                                <sspu:JButton runat="server" ID="JButton_UseUploadify" Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                            </ItemContent>
                        </TabItem>
                        <TabItem TabName="关联文件显示或下载">
                            <ItemContent>



                                <div class="ContentBoxRow">
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div class="ContentBoxColumn">
                                                <div>数据来源：</div>
                                                <sspu:JDropDown runat="server" ID="JDropDown_ShowFileOrLinkDataSource"></sspu:JDropDown>
                                            </div>
                                            <div class="ContentBoxColumn">
                                                <div>类型：</div>
                                                <sspu:JDropDown runat="server" ID="JDropwn_ShowFileOrLink">
                                                    <Item Text="显示为图片"></Item>
                                                    <Item Text="显示为下载链接"></Item>
                                                </sspu:JDropDown>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div>显示为图片宽度：</div>
                                            <sspu:JTextBox runat="server" Width="175" ID="JTextBox_ShowFileAsImageWidth" Text="" />
                                            <div style="margin-left: 5px;">显示为图片高度：</div>
                                            <sspu:JTextBox runat="server" Width="175" ID="JTextBox_ShowFileAsImageHeight" Text="" />
                                        </div>

                                    </div>
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div>下载链接文字：</div>
                                            <sspu:JTextBox runat="server" Width="300" ID="JTextBox_ShowFileAsDownloadText" Text="" />
                                        </div>

                                    </div>
                                </div>
                                <sspu:JButton runat="server" ID="JButton_UseShowFileAsImgeOrLink" Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                            </ItemContent>
                        </TabItem>
                        <TabItem TabName="数据树类型">
                            <ItemContent>
                                <div class="ContentBoxRow">
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div>可多选：</div>
                                            <div>
                                                <sspu:JCheckBox NeedClientData="True" RemainCheckState="True" runat="server" ID="JCheckBox_DataTreeEnableMultiSelect" CssClass="CheckBoxControl" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="ContentBoxRow">
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div>手动输入数据源，以逗号分割：</div>
                                            <sspu:JTextBox runat="server" Width="300" ID="JTextBox_InputByTreeSourceManule" Text="" />
                                        </div>
                                    </div>
                                </div>
                                <div class="ContentBoxRow">
                                    <div class="box">
                                        <div class="ContentBoxColumn">
                                            <div>从其它表中搜索：</div>
                                            <sspu:JDropDown runat="server" ID="JDropDown_InputByTreeSourceByTable"></sspu:JDropDown>
                                            <div style="margin-left: 5px;">数据过滤：</div>
                                            <sspu:JTextBox runat="server" Width="300" ID="JTextBox_InputByTreeSourceByTableDataFilter" />
                                        </div>
                                    </div>
                                </div>
                                <sspu:JButton runat="server" ID="JButton_UseInputByTreeSource" Text="确定" CssClass="ButtonSubmit" OnClick="JButtonClicked" />
                            </ItemContent>
                        </TabItem>
                    </sspu:JTabs>


                </div>

            </div>
        </asp:Panel>



    </div>
</asp:Content>
