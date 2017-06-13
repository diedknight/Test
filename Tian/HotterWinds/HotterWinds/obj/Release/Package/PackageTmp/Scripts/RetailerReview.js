var NewRatingController = {

    creatNew: function (parentId, subClass, setValId, setTilteId) {

        NewObj = {};
        (function ($$) {

            var myRating = -1;

            var myParentId;
            var mySubClass;
            var mySetValId;
            var mySetTilteId;

            $$.init = function (parentId, subClass, setValId, setTilteId) {

                myParentId = parentId;
                mySubClass = subClass;
                mySetValId = setValId;
                mySetTilteId = setTilteId;

                bindEvent();
            }

            var bindEvent = function () {
                $("#" + myParentId + " ." + mySubClass).each(function (index) {
                    var val = index + 1;
                    $(this).on("click", function () {
                        setTitle(val);
                        myRating = val;
                        $("#" + mySetValId).val(myRating);
                        setReviewStar(myRating);
                    });

                    $(this).on("mouseover", function () {
                        setReviewStar(val);
                        setTitle(val);
                    });

                    $(this).on("mouseout", function () {
                        setReviewStar(myRating);
                        setTitle(myRating);
                    });
                });
            }

            var setTitle = function (val) {
                var selectVal;
                switch (val) {
                    case 1:
                        selectVal = " Very poor";
                        break;
                    case 2:
                        selectVal = " Poor";
                        break;
                    case 3:
                        selectVal = " OK";
                        break;
                    case 4:
                        selectVal = " Good";
                        break;
                    case 5:
                        selectVal = " Excellent";
                        break;
                    case 0:
                        selectVal = "";
                        break;
                    case -1:
                        selectVal = "";
                        break;
                    default:
                        selectVal = val;
                }

                $("#" + mySetTilteId).text(selectVal);
            }

            var clearRating = function () {
                $("#" + myParentId + " ." + mySubClass + " span").each(function () {
                    $(this).removeClass("ratover").addClass("ratout");
                });
            }

            var setReviewStar = function (val) {
                clearRating();
                $("#" + myParentId + " ." + mySubClass).each(function (index) {
                    if (index < val) {
                        $(this).find("span").removeClass("ratout").addClass("ratover");
                    }
                });
            }

        })(NewObj);

        NewObj.init(parentId, subClass, setValId, setTilteId);
        return NewObj;
    }

};