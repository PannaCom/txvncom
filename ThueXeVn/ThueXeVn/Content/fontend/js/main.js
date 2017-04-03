﻿$(window).on('load', function () {
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

        $(document).ready(function () {
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


            //share social                
            //share this
            if ($('#sharethis').length) {
                $('#sharethis').on('click', function () {
                    var html = "<form id=\"frmShareLink\">"
+ "<div class=\"form-horizontal\">"
+ "<div class=\"form-group\">"
+ "<div class=\"input-group\">"
+ "<input type=\"text\" class=\"form-control\" id=\"inputShareLink\" name=\"inputShareLink\" disabled>"
+ "<span class=\"input-group-btn\">"
+ "<button type=\"button\" class=\"btn btn-default\" id=\"btnShareLink\"><i class=\"fa fa-copy\"></i></button>"
+ "</span>"
+ "</div>"
+ "</div>"
+ "</div>"
+ "</form>";
                    $('#modal_sharesocial .modal-body').empty().html(html);
                    $('#modal_sharesocial .modal-header').find('.modal-title').empty().html("Sao chép liên kết bên dưới vào khay nhớ tạm thời của bạn để chia sẻ những kết quả này");
                    if ($('#inputShareLink').length) {
                        var url = window.location.href;
                        $('#inputShareLink').val(url);
                    }
                    $('#modal_sharesocial').modal('show', { backdrop: 'static', keyboard: false });
                    if ($('#btnShareLink').length) {
                        var copyTextareaBtn = document.querySelector('#btnShareLink');

                        copyTextareaBtn.addEventListener('click', function (event) {
                            var copyTextarea = document.querySelector('#inputShareLink');
                            copyTextarea.select();

                            try {
                                var successful = document.execCommand('copy');
                                var msg = successful ? 'Đã sao chép để chia sẻ' : 'unsuccessful';
                                notifysucc(msg);
                            } catch (err) {
                                console.log('Oops, unable to copy');
                            }
                        });

                    }

                });
            }

            //share email

            //share facebook
            if ($('.social-share').length) {
                var t=$('[data-js="share"]').attr("data-permalink"),e=encodeURIComponent(
		$('[data-js="share"]').attr("data-title"));
                bitly(t,function(){
                    $('[data-js="share-twitter"]').attr("href","https://twitter.com/share?text="+e+"%20-&url="+t+"&via=ngnguyenvannam&related=ngnguyenvannam"),
                    $('[data-js="share-google"]').attr("href","https://plus.google.com/share?url="+t)
                }),

                $('[data-js="share-twitter"]').click(function(){
                    return window.open($(this).attr("href"),"sharer","toolbar=0,status=0,width=620,height=430"),!1}),
                $('[data-js="share-google"]').click(function(){
                    return window.open($(this).attr("href"),"sharer","toolbar=0,status=0,width=620,height=430"),!1})
            }

            if ($('#sharefacebook').length) {
                $('#sharefacebook').on('click', function () {
                    FB.ui({
                        method: 'share',
                        mobile_iframe: true,
                        href: $(this).attr('data-href'),
                    }, function (response) { });
                })
                
            }


            $(document).on("scroll", function () {
                if ($(document).scrollTop() > 200) {
                    $(".search_options").addClass("navbar-fixed-top");
                } else {
                    $(".search_options").removeClass("navbar-fixed-top");
                }
            });

            $('img.img-driver-item').zoomify({ duration: 1000 }); // 1s duration

            
        });




    }());


}
main();

/**function bitly(t, e) {
//    var a = "o_21307vlum1", n = "R_e3ba276d5631459d9bed062157f85f1a";
//    $.getJSON("https://api-ssl.bitly.com/v3/shorten?callback=?",
//		{ format: "json", apiKey: n, login: a, longUrl: t },
//		function (t) { e(t.data.url) })
//}*/

function bitly(t, e) {
    var a = "o_21307vlum1", n = "R_e3ba276d5631459d9bed062157f85f1a";
    $.getJSON("https://api-ssl.bitly.com/v3/shorten?callback=?",
		{ format: "json", apiKey: n, login: a, longUrl: t },
		function (t) { e(t.data.url) })
}

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

toastr.options = {
    timeOut: 0,
    positionClass: "toast-top-center"
};

function notifywarn(msg) {
    toastr.clear();
    var notify = toastr.warning(msg);

    var $notifyContainer = jQuery(notify).closest('.toast-top-center');
    if ($notifyContainer) {
        // align center
        var containerWidth = jQuery(notify).width() + 20;
        $notifyContainer.css("margin-left", -containerWidth / 2);
    }
}

function notifysucc(msg) {
    toastr.clear();
    var notify = toastr.success(msg);

    var $notifyContainer = jQuery(notify).closest('.toast-top-center');
    if ($notifyContainer) {
        // align center
        var containerWidth = jQuery(notify).width() + 20;
        $notifyContainer.css("margin-left", -containerWidth / 2);
    }
}

var addUrlParam = function (search, key, val) {
    var newParam = key + '=' + val,
        params = '?' + newParam;

    // If the "search" string exists, then build params from it
    if (search) {
        // Try to replace an existance instance
        params = search.replace(new RegExp('([?&])' + key + '[^&]*'), '$1' + newParam);

        // If nothing was replaced, then add the new param to the end
        if (params === search) {
            params += '&' + newParam;
        }
    }
    return params;
};

function getsearchOption(s, i) {
    var url = "";
    if (s === "giaxe") {
        switch (i) {
            case "1":
                url = document.location.pathname + addUrlParam(document.location.search, 'gia_select', 1);
                break;
            case "2":
                url = document.location.pathname + addUrlParam(document.location.search, 'gia_select', 2);
                break;
            default:
                break;
        }
    } else if (s === "nhaxe") {
        switch (i) {
            case "1":
                url = document.location.pathname + addUrlParam(document.location.search, 'nhaxe', 1);
                break;
            case "0":
                url = document.location.pathname + addUrlParam(document.location.search, 'nhaxe', 0);
                break;
            default:
                break;
        }
    }
    //console.log(url);
    window.location.href = url;
}

function daydiff(first, second) {
    return Math.abs((second - first) / (1000 * 60 * 60 * 24));
}

function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[1] - 1, mdy[0]);
}



/*alert(daydiff(parseDate($('#first').val()), parseDate($('#second').val())));*/