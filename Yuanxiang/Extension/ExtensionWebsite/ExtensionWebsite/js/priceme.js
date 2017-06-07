function pctoolbarclose() {
    var bar = document.getElementById("pc_toolbar");
    bar.setAttribute("style", "display:none;");
    var css = document.body.className();
    css = css.replace(" pc_toolbar_visible", "");
    document.body.className = css;
}

function on_clickOut(ordid, pid, rid, rpid, pname, catId, price, other, countryID, redirect) {
    var url = redirect + "/ResponseRedirect.aspx?pid=" + pid + "&rid=" + rid + "&rpid=" + rpid + "&countryID=" + countryID + "&cid=" + catId + other;
    location.href = url;
}