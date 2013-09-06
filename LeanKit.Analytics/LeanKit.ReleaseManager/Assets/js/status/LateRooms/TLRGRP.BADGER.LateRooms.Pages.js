(function () {
    TLRGRP.BADGER.Pages.register('UserJourney', {
            id: 'HomePage',
            name: 'Home Page',
            pagetype: 'home-page'
        },
        {
            id: 'Search',
            pagetype: 'search',
            regex: 'Search|(H|h)otels'
        },
        {
            id: 'HotelDetails',
            name: 'Hotel Details',
            pagetype: 'hotel-details',
            regex: 'hotel-reservations'
        },
        {
            id: 'BookingForm',
            name: 'Booking Form',
            pagetype: 'booking-form',
            regex: '(BookingError/LogError\.mvc|Booking/Online|HotelReservationsSubmit/Submit|Booking/Submit)'
        });
})();