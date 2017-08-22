<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewHeaderControl.ascx.cs" Inherits="HotterWinds.Modules.Common.NewHeaderControl" %>

<%@ Import Namespace="PriceMeCommon.Extend" %>

<style type="text/css">
    .search_box_extension {
        border: 1px solid #c6c6c6;
        border-radius: 0px;
    }

        .search_box_extension .input-group-btn {
            background-color: white;
        }

        .search_box_extension .search-textbox {
            box-shadow: none;
        }

        .search_box_extension .input-group-btn .search-btn {
            border-radius: 0px;
            background-color: white;
            border: none;
            border-left: solid 1px #c6c6c6;
            background-image: none;
        }

        .search_box_extension .glyphicon-search:before {
            color: black;
            font-size: 18px;
            line-height: 28px;
            position: relative;
            top: -3px;
        }

    #headerV1 {
        border-bottom: none;
    }

    #suggest-div {
        /*width: 96% !important;*/
        z-index: 1000;
        position: absolute;
        border-top: 2px solid #ccc;
        border-radius: 5px;
        margin-top: -10px;
    }

        #suggest-div li strong {
            color: #1fc0a0;
        }

    @media (min-width: 768px) {
        .search_box_extension {
            width: 480px;
        }

        #suggest-div {
            width: 480px;
        }

            #suggest-div #suggestULRight {
                width: 480px;
            }
    }

    @media (min-width: 1008px) {
        .search_box_extension {
            width: 735px;
        }

        #suggest-div {
            width: 735px;
        }

            #suggest-div #suggestULLeft {
                width: 735px;
            }

            #suggest-div #suggestULRight {
                width: 735px;
            }
    }

    @media (min-width: 1200px) and (max-width:1500px) {
        .search_box_extension {
            width: 875px;
        }

        #suggest-div {
            width: 875px;
        }

            #suggest-div #suggestULLeft {
                width: 480px;
            }

            #suggest-div li .aaaa {
                width: 380px;
            }

            #suggest-div #suggestULRight {
                width: 310px;
            }
    }

    @media (min-width: 1500px) {

        .search_box_extension {
            width: 875px;
        }

        #suggest-div {
            width: 875px;
        }

            #suggest-div #suggestULLeft {
                width: 480px;
            }

            #suggest-div li .aaaa {
                width: 380px;
            }

            #suggest-div #suggestULRight {
                width: 310px;
            }
    }
</style>

<%--<div class="search-box">
    <input class="mgksearch" type="text" value="" placeholder="Search entire store here..." maxlength="70" name="s">
    <button class="search-btn-bg" type="submit">&nbsp;</button>
</div>--%>
<div id="headerV1">

    <%if (isLogin)
        { %>
    <style type="text/css">
        @media (min-width: 1007px) {
            .new-searchbar {
                width: 435px !important;
            }
        }

        @media (min-width: 1200px) {
            .new-searchbar {
                width: 610px !important;
            }
        }
    </style>
    <%} %>

    <div class="container navbar-default" itemscope itemtype="https://schema.org/Organization">

        <div id="header-right" class="float-right" style="float: left;">
            <div class="collapse navbar-collapse" id="top-navbar-collapse">
                <div class="new-searchbar input-group navbar-left search_box_extension" id="in-searchbar">
                    <input class="form-control search-textbox" type="text" id="SearchTextBox" runat="server" />
                    <span class="input-group-btn">
                        <button id="btnSearch" type="button" class="btn btn-info search-btn" onclick="search('<%=SearchTextBox.ClientID%>')" title="Search for products from online shops"><span class="glyphicon glyphicon-search"></span></button>
                    </span>
                </div>
            </div>
        </div>

        <div class="new-searchbar input-group" id="out-searchbar" style="display: none;">
        </div>
    </div>

</div>

<div id="suggest-div" class="container" style="display: none;">
    <ul id="suggestULLeft"></ul>
    <ul id="suggestULRight"></ul>
    <span class="glyphicon glyphicon-remove-sign"></span>
</div>
<script type="text/javascript">
    SuggestHelper.init("<%=SearchTextBox.ClientID %>", "suggest-div", "suggestULLeft", "suggestULRight");
    setFocus(document.getElementById('<%=SearchTextBox.ClientID%>'));
</script>
