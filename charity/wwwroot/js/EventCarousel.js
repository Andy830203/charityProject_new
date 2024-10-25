function bindCarousel() {
    
    let owl = $('.owl-carousel').owlCarousel({
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
    
    //owl.trigger('refresh.owl.carousel');
    //$('.owl-carousel').trigger('refresh.owl.carousel');

    /*console.log('hi');*/

    //$('.accordion-header').on('click', function () {
    //    if (!$(this).hasClass() {

    //    }
    //    console.log('hi');
    //    console.log(`${$(this).text()}`);
    //    console.log(`${$(this).html()}`);
    //    //console.log(this);
    //});

    // 監聽手風琴展開事件，當手風琴展開時，刷新對應的 Owl Carousel
    $(document).on('shown.bs.collapse', '.accordion-collapse', function (e) {
        var owlCarousel = $(e.target).find('.owl-carousel');
        if (owlCarousel.length) {
            // 刷新該展開區塊中的 Carousel，防止跑版
            owlCarousel.trigger('refresh.owl.carousel');
        }
    });
}
