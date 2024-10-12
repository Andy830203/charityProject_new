function bindCarousel() {
    $('.owl-carousel').owlCarousel({
        autoWidth: true,
        nav: true,
        dots: true,
        items: 2,
        margin: 10,
        lazyLoad: true
    });

    $('.click-img').each(function () {

        console.log(typeof this);
        console.log($(this).html());
        console.log($(this));
    });
}
