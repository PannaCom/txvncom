$(window).on('load', function () {
    $('html,body').animate({ scrollTop: 0 });
});

function main() {

    (function () {
        'use strict';

        if ($('#tf-home').length) {
        	$('#tf-home').filter(function(index) {
        		return $("img", this ).length === 1;
        	}).addClass("img-responsive");
        }

        // Smooth Scrolling
        //==========================================
        $(function () {
            $('a.scroll').click(function () {
                if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                    var target = $(this.hash);
                    target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                    if (target.length) {
                        $('html,body').animate({
                            scrollTop: target.offset().top - 50
                        }, 1000);
                        return false;
                    }
                }
            });
        });

        /*====================================
        Script for the Counters for Facts Section
        ======================================*/
        $('.count').each(function () {
            var $this = $(this);
            $this.data('target', parseInt($this.html()));
            $this.data('counted', false);
            $this.html('0');
        });
        $(window).bind('scroll', function () {
            var speed = 3000;
            $('.count').each(function () {
                var $this = $(this);
                if (!$this.data('counted') && $(window).scrollTop() +
                    $(window).height() >= $this.offset().top) {
                    $this.data('counted', true);
                    $this.animate({
                        dummy: 1
                    }, {
                        duration: speed,
                        step: function (now) {
                            var $this = $(this);
                            var val = Math.round(
                                $this.data(
                                    'target') *
                                now);
                            $this.html(val);
                            if (0 < $this.parent(
                                '.value').length) {
                                $this.parent(
                                    '.value').css(
                                    'width',
                                    val + '%');
                            }
                        }
                    });
                }
            });
        }).triggerHandler('scroll');

        /*====================================
        Portfolio Carousel 
        ======================================*/
        //$(document).ready(function () {
        //    var owl = $("#team");
        //    owl.owlCarousel({

        //        itemsCustom: [
        //          [0, 1],
        //          [450, 1],
        //          [660, 2],
        //          [700, 2],
        //          [1200, 3],
        //          [1600, 3]
        //        ],
        //        navigation: false,
        //        pagination: true,
        //    });

        //});

        /*====================================
        Portfolio Isotope Filter
        ======================================*/
        //$(window).load(function () {
        //    var $container = $('#itemsWork , #itemsWorkTwo, #itemsWorkThree');
        //    $container.isotope({
        //        filter: '* , all',
        //        animationOptions: {
        //            duration: 750,
        //            easing: 'linear',
        //            queue: false
        //        }
        //    });
        //    $('.cat a').click(function () {
        //        $('.cat .active').removeClass('active');
        //        $(this).addClass('active');
        //        var selector = $(this).attr('data-filter');
        //        $container.isotope({
        //            filter: selector,
        //            animationOptions: {
        //                duration: 750,
        //                easing: 'linear',
        //                queue: false
        //            }
        //        });
        //        return false;
        //    });

        //});

        /*====================================
        Nivo Lightbox 
        ======================================*/
        // Agency Portfolio Popup
        //$('#itemsWork a , #itemsWorkTwo a , #itemsWorkThree a , #popup a').nivoLightbox({
        //    effect: 'slideDown',
        //    keyboardNav: true,
        //});

        $(document).ready(function () {

            //$("#owl-demo").owlCarousel({

            //    navigation: false, // Show next and prev buttons
            //    slideSpeed: 300,
            //    paginationSpeed: 400,
            //    singleItem: true

            //    // "singleItem:true" is a shortcut for:
            //    // items : 1, 
            //    // itemsDesktop : false,
            //    // itemsDesktopSmall : false,
            //    // itemsTablet: false,
            //    // itemsMobile : false

            //});

            //$.ajaxSetup({
            //    beforeSend: function (jqXHR, options) {
            //        setTimeout(function () {
            //            // null beforeSend to prevent recursive ajax call
            //            $.ajax($.extend(options, { beforeSend: $.noop }));
            //        }, 5000);
            //        return false;
            //    }
            //});

            if ($('#menu').length) {
                $('#menu').affix({
                    offset: {
                        top: $('#tf-menu').outerHeight()
                    }
                });
                // This event fires immediately before the element has been affixed
                $('#menu').on('affix.bs.affix', function () {
                    $('#menu').addClass('navbar-fixed-top');
                    $('#tf-menu').css('margin-top', '49px');
                    $('#tf-home').css('background-position', '0px 0px');
                    //$('#menu').children('div').children('ul').append("<li class='disable_hover' id='hotlineadd'><a href='tel:0964108688' style='margin: 2px; color: #333;'>Hỗ trợ kỹ thuật: <span class='btn btn-primary btn-success'>096 410 8688</span></a></li>");
                });

                // This event fires immediately before the element has been affixed-top.
                $('#menu').on('affix-top.bs.affix', function () {
                    $('#menu').removeClass('navbar-fixed-top');
                    $('#tf-menu').css('margin-top', '0');
                    $('#tf-home').css('background-position', '0 117px');
                    //$('#menu').children('div').children('ul').find('#hotlineadd').remove();
                });
            }

            if ($('.phone').length) {
                $('.phone').text(function (i, text) {
                    return text.replace(/(\d\d\d\d)(\d\d\d)(\d\d\d)/, '$1.$2.$3');
                });
            }

            $('ul.nav li.dropdown').hover(function () {
                $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(500);
            }, function () {
                $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(500);
            });

            // scroll top
            //==========================================    
            $(window).scroll(function () {
                $(this).scrollTop() > 500 ? $('.totop').fadeIn() : $('.totop').fadeOut();
            });
            if ($('.totop').length) {
                $('.totop').click(function () {
                    $('html,body').animate({ scrollTop: 0 }, 500);
                    return false;
                })
            }
            

        });




    }());


}
main();

function logout() {
    window.location.href = "/Home/logouttaixe";
}


// sửa lỗi call back facebook location hash
function facebookCallback() {
    if (window.location.hash == '#_=_') {
        history.replaceState
            ? history.replaceState(null, null, window.location.href.split('#')[0])
            : window.location.hash = '';
    }
}
facebookCallback();

function ctv_dangxuat() {
    if (confirm('Bạn chắc chắn muốn đăng xuất?')) {
        window.location.href = "/Home/ctv_dang_xuat";
    }
}

function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}