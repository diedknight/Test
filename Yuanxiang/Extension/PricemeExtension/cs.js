
var url = document.location.href;
var stringjs;

$.ajax({
    url: 'https://api.priceme.co.nz/Extension/pricemeextension.aspx?key=' + encodeURIComponent(url),
    type: 'GET',
    dataType: 'html',
    success: function (html) {
        if (html.indexOf('id="pc_toolbar"') > 0) {
            var div = document.createElement("div");
            div.innerHTML = html;
            document.body.appendChild(div);
            var css = document.body.className;
            document.body.className = css + " pc_toolbar_visible";

            PcJavaScript();
            var js = document.createElement("script");
            js.setAttribute("type", "text/javascript");
            js.innerHTML = stringjs;
            document.head.appendChild(js);
        }
    }
});

function PcJavaScript() {
    stringjs = 'function pctoolbarclose() {var bar = document.getElementById("pc_toolbar");bar.setAttribute("style", "display:none;");'
            + 'var css = document.body.className; css = css.replace(" pc_toolbar_visible", ""); document.body.className = css; }'
            + 'function on_clickOut(ordid, pid, rid, rpid, pname, catId, price, other, countryID, redirect) { var url = redirect + "/ResponseRedirect.aspx?pid=" + pid + "&rid=" + rid + "&rpid=" + rpid + "&countryID=" + countryID + "&cid=" + catId + other + "&aid=38";'
            + 'location.href = url; }';
}