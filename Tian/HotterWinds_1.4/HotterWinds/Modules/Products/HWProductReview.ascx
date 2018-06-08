<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HWProductReview.ascx.cs" Inherits="HotterWinds.Modules.Products.HWProductReview" %>
<div id="reviews">
    <div id="comments">
        <h2>Reviews</h2>

        <%if (this.ProductReviewList != null && this.ProductReviewList.Count != 0) %>
        <%{ %>
        <ol id="reviewList1" class="commentlist">
            <%foreach (var review in this.ProductReviewList) %>
            <%{ %>
            <li itemprop="review" itemscope="" itemtype="http://schema.org/Review" class="comment byuser comment-author-123 even thread-even depth-1">

                <div class="comment_container">

                    <img alt="" src="http://2.gravatar.com/avatar/b2d7d2d13aed54c2ed7feb538b382b42?s=60&amp;d=mm&amp;r=g" srcset="http://2.gravatar.com/avatar/b2d7d2d13aed54c2ed7feb538b382b42?s=120&amp;d=mm&amp;r=g 2x" class="avatar avatar-60 photo" height="60" width="60">
                    <div class="comment-text" style="padding-top:0.5em;">
                        <div class="rating" style="float: right;">
                            <div class="ratings" style="cursor: default;">
                                <div class="rating-box">
                                    <div rating="<%=review.Rating %>" style="cursor: default; width: <%=(review.Rating * 2 * 10) %>%" class="rating score"></div>
                                </div>
                            </div>
                        </div>

                        <div itemprop="description" class="description">
                            <p><span style="color:#808080; font-weight:bold; font-size:12px;"><%=review.UserName %></span> - <span style="color:#808080; font-size:12px;"><%=review.PostDate.ToString("MMMM dd , yyyy") %></span></p>
                            <p><%=review.Body %></p>
                        </div>
                    </div>
                </div>
            </li>
            <%} %>
        </ol>
        <%} %>
        <%else %>
        <%{ %>
        <ol id="reviewList1" class="commentlist">
            
        </ol>

        <p id="notreviews" class="woocommerce-noreviews">There are no reviews yet.</p>
        <%} %>
    </div>

    <%if (this.UserData != null) %>
    <%{ %>
    <div id="review_form_wrapper">
        <div id="review_form">
            <div id="respond" class="comment-respond">
                <h3 id="reply-title" class="comment-reply-title">Add a review <small><a rel="nofollow" id="cancel-comment-reply-link" href="/linea/product/trendy-trotters-mens-sports/#respond" style="display: none;">Cancel reply</a></small></h3>

                <p class="comment-form-rating">
                    <label for="rating">Your Rating</label>

                    <%--<asp:DropDownList ID="rating" ClientIDMode="Static" runat="server" style="display:none">
                        <asp:ListItem Value="0">Rate…</asp:ListItem>
                        <asp:ListItem Value="5">Perfect</asp:ListItem>
                        <asp:ListItem Value="4">Good</asp:ListItem>
                        <asp:ListItem Value="3">Average</asp:ListItem>
                        <asp:ListItem Value="2">Not that bad</asp:ListItem>
                        <asp:ListItem Value="1">Very Poor</asp:ListItem>
                    </asp:DropDownList>--%>

                    <select name="rating" id="rating" style="display: none;">
                        <option value="0">Rate…</option>
                        <option value="5">Perfect</option>
                        <option value="4">Good</option>
                        <option value="3">Average</option>
                        <option value="2">Not that bad</option>
                        <option value="1">Very Poor</option>
                    </select>

                </p>

                <p class="comment-form-comment">
                    <label for="txtName">Your Name</label>
                    <br />
                    <input id="txtYourName" name="txtYourName" value="" style="border:1px solid #ddd" />
                </p>

                <p class="comment-form-comment">
                    <label for="comment">Your Review</label>
                    <textarea id="comment" name="comment" cols="45" rows="8" style="padding: 8px;"></textarea>
                    <%--<asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" cols="45" rows="8"></asp:TextBox>--%>
                </p>
                <p class="form-submit">

                    <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" style="background:#1fc0a0 !important; color:#fff !important" />--%>

                    <input id="submit" type="button" class="submit" value="Submit" />
                    <input type="hidden" value="<%=Product.ProductID %>" id="txtProductId" />
                    <input type="hidden" value="<%=this.UserData.name %>" id="txtUserName" />

                    <%--<input name="submit" type="submit" id="submit" class="submit" value="Submit">
                    <input type="hidden" name="comment_post_ID" value="799" id="comment_post_ID">
                    <input type="hidden" name="comment_parent" id="comment_parent" value="0">--%>
                </p>

            </div>
            <!-- #respond -->
        </div>
    </div>
    <%} %>
    <%else %>
    <%{ %>
    <div id="review_form_wrapper">
        <div id="review_form">
            <div id="respond" class="comment-respond">
                <h3 id="reply-title" class="comment-reply-title">Be the first to review &ldquo;<%=Product.ProductName %>&rdquo; <small><a rel="nofollow" id="cancel-comment-reply-link" href="/linea/product/today-fashion-casual-sleeveless-solid-womens-top-light-pink/#respond" style="display: none;">Cancel reply</a></small></h3>
                <p class="must-log-in">You must be <a href="/Login.aspx">logged in</a> to post a review.</p>
            </div>
            <!-- #respond -->
        </div>
    </div>
    <%} %>

    <div class="clear"></div>
</div>

<script type="text/javascript">
    $("#submit").on("click", function () {
        var data = {
            "comment": $("#comment").val(),
            "rating": $("#rating").val(),
            "pid": $("#txtProductId").val(),
            "name": $("#txtUserName").val(),
            "yourName": $("#txtYourName").val()
        };

        if (data.comment == "") {
            alert("please type a comment.");
            return;
        }

        if (data.yourName == "") {
            alert("please type a name.");
            return;
        }


        GlobalAjax("AjaxDefaultController", "AddReview", data, function (msg) {
            
            if (msg != "0") {
                $("#notreviews").remove();

                var node = $("#reviewList1");
                var template = "";
                template += "<li itemprop=\"review\" itemscope=\"\" itemtype=\"http://schema.org/Review\" class=\"comment byuser comment-author-123 even thread-even depth-1\">";
                template += "<div class=\"comment_container\">";
                template += "<img alt=\"\" src=\"http://2.gravatar.com/avatar/b2d7d2d13aed54c2ed7feb538b382b42?s=60&amp;d=mm&amp;r=g\" srcset=\"http://2.gravatar.com/avatar/b2d7d2d13aed54c2ed7feb538b382b42?s=120&amp;d=mm&amp;r=g 2x\" class=\"avatar avatar-60 photo\" height=\"60\" width=\"60\">";
                template += "<div class=\"comment-text\">";
                template += "<div class=\"rating\" style=\"float: right;\">";
                template += "<div class=\"ratings\" style=\"cursor: default;\">";
                template += "<div class=\"rating-box\">";
                template += "<div rating=\"" + data.rating + "\" style=\"cursor: default; width: " + (data.rating * 2 * 10) + "%\" class=\"rating score\"></div>";
                template += "</div>";
                template += "</div>";
                template += "</div>";
                template += "<div itemprop=\"description\" class=\"description\">";
                template += "<p><span style=\"color:#808080; font-weight:bold; font-size:12px;\">" + data.yourName + "</span> - <span style=\"color:#808080; font-size:12px;\">" + msg + "</span></p>";
                template += "<p>" + data.comment + "</p>";
                template += "</div>";
                template += "</div>";
                template += "</div>";
                template += "</li>";

                node.append(template);

                var count = $("#reviewList1>li").length;
                var average = 0;

                $("#reviewList1 > li .rating.score").each(function () {                    
                    var tempNode = $(this);
                    average += tempNode.attr("rating") * 1.0;
                });

                average = average / count;                

                $("#tab_reviews").text("Reviews (" + count + ")");
                $(".product-essential .price-block .rating").attr("style", "width: " + (average * 2 * 10) + "%");
                $(".product-essential .price-block .rating").parent().next().text(count + " reviews");


            }

        });

    });
</script>
