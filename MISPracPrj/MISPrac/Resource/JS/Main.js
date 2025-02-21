$(function () {

    $('li.treeViewIsSelected').each(function () {
        $(this).children('a').first().css({ "font-weight": "bold", 'color':'Red'});
    });

    //$('#ConfigNavTableContainer').css("width", (parseInt($('.pageContent').css("width"), 10) - 810) + "px");

    
    
});


(function ($) {
    $(document).ready(function () {
        $('#JMenu1').prepend('<div id="menu-button">Menu</div>');
        $('#JMenu1 #menu-button').on('click', function () {
            var menu = $(this).next('ul');
            if (menu.hasClass('open')) {
                menu.removeClass('open');
            }
            else {
                menu.addClass('open');
            }
        });
    });
})(jQuery);



