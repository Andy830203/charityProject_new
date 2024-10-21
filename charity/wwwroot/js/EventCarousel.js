function bindCarousel() {
    $('.owl-carousel').owlCarousel({
        autoWidth: true,
        nav: true,
        dots: true,
        items: 2,
        margin: 10,
        lazyLoad: true,
        loop: true,
        animateOut: false, // 禁用動畫效果
        animateIn: false
    });

    //$('.owl-carousel').trigger('refresh.owl.carousel');

    $('.accordion-header').on('click', function () {

    });
}
