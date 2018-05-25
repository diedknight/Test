<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HWHeaderTop.ascx.cs" Inherits="HotterWinds.Modules.HWHeaderTop" %>

<%if (this.UserData == null) %>
<%{ %>
<div class="header-container">
    <div class="container">
        <div class="row">
            <!-- Header Top Links -->
            <div class="col-xs-12 col-sm-8 col-md-6 col-lg-6 pull-right">
                <div class="toplinks">
                    <div class="links">
                        <ul id="menu-toplinks" class="top-links1 mega-menu1">
                            <li id="nav-menu-item-3284" class="menu-item menu-item-type-post_type menu-item-object-page  narrow "><a href="/blog/" class="">Blog</a></li>
                            <li class="menu-item"><a href="/Login.aspx"><span>Login</span></a></li>
                        </ul>
                    </div>
                </div>
                <!-- End Header Top Links -->
            </div>
        </div>
    </div>
</div>
<%} %>
<%else %>
<%{ %>
<div class="header-container">
    <div class="container">
        <div class="row">
            <!-- Header Language -->
            <div class="col-xs-12 col-sm-4 col-md-6 col-lg-6 pull-left">
                <%--<div class="dropdown block-language-wrapper">
                    <a role="button" data-toggle="dropdown" data-target="#" class="block-language dropdown-toggle" href="#">
                        <img src="http://wordpress.magikthemes.com/linea/wp-content/themes/linea/images/english.png" alt="English">
                        English <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu" role="menu">
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">
                            <img src="http://wordpress.magikthemes.com/linea/wp-content/themes/linea/images/english.png" alt="English">
                            English</a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">
                            <img src="http://wordpress.magikthemes.com/linea/wp-content/themes/linea/images/francais.png" alt="French">
                            French </a></li>
                        <li role="presentation"><a role="menuitem" tabindex="-1" href="#">
                            <img src="http://wordpress.magikthemes.com/linea/wp-content/themes/linea/images/german.png" alt="German">
                            German</a></li>
                    </ul>
                </div>
                <div class="dropdown block-currency-wrapper">
                    <a role="button" data-toggle="dropdown" data-target="#" class="block-currency dropdown-toggle" href="#">USD <span class="caret"></span></a>
                    <ul class="dropdown-menu" role="menu">
                        <li role="presentation">
                            <a role="menuitem" tabindex="-1" href="#">$ - Dollar                </a>
                        </li>
                        <li role="presentation">
                            <a role="menuitem" tabindex="-1" href="#">£ - Pound                </a>
                        </li>
                        <li role="presentation">
                            <a role="menuitem" tabindex="-1" href="#">€ - Euro                </a>
                        </li>
                    </ul>
                </div>--%>
                <div class="welcome-msg">Logged in as   <b><%=this.UserData.name %></b> </div>
            </div>
            <!-- Header Top Links -->
            <div class="col-xs-12 col-sm-8 col-md-6 col-lg-6 pull-right hidden-xs">
                <div class="toplinks">
                    <div class="links">
                        <ul id="menu-toplinks" class="top-links1 mega-menu1">                            
                            <li id="nav-menu-item-3416" class="menu-item menu-item-type-post_type menu-item-object-page  narrow "><a href="http://wordpress.magikthemes.com/linea/blog/" class="">Blog</a></li>                            
                            <li class="menu-item"><span><a href="/Logout.aspx">Logout</a></span></li>
                        </ul>
                    </div>
                </div>
                <!-- End Header Top Links -->
            </div>
        </div>
    </div>
</div>
<%} %>