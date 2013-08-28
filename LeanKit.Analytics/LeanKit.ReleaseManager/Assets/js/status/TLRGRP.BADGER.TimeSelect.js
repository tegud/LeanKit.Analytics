(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.TimeSelect = function (element, currentTimePeriod) {
        var request = new TLRGRP.BADGER.MetricsRequest();
        
        if (typeof currentTimePeriod === 'string') {
            currentTimePeriod = {
                timePeriod: currentTimePeriod
            };
        }
        
        function buildUrl(timeLimit) {
            var metric = request.dashboard();
            var subMetric = request.subMetric();
            
            if (metric && subMetric) {
                return '/Status?metric=' + metric + '&subMetric=' + subMetric + '&' + timeLimit;
            }
            else if (metric) {
                return '/Status?metric=' + metric + '&' + timeLimit;
            }

            return '/Status?' + timeLimit;
        }

        function selectTimePeriod(element, timePeriodOverride) {
            var currentTimePeriodString = currentTimePeriod.start ? '' : currentTimePeriod.timePeriod;

            if (timePeriodOverride) {
                currentTimePeriodString = timePeriodOverride;
            }

            element
                .children().each(function() {
                    var currentItem = $(this);

                    if (currentItem.data('timeLimit') === 'timePeriod=' + currentTimePeriodString) {
                        currentItem.prop('selected', true);
                        return false;
                    }
                    
                    if (!currentItem.data('timeLimit') && currentTimePeriod.start) {
                        currentItem
                            .prop('selected', true)
                            .text('User Defined');

                        if (element.children('.custom-item').size() === 1) {
                            element.append('<option class="custom-item">Adjust...</option>');
                        }
                        return false;
                    }

                    return true;
                });
        }
        
        function setCustomPeriod() {
            var timePeriodSelect = element
                .clone()
                .children('.custom-item')
                .remove()
                .end()
                .children()
                .each(function () {
                    var item = $(this);

                    item.text(item.data('customText'));
                })
                .end();
            var startDate = '';
            var startTime = '';

            if (currentTimePeriod.start) {
                var startMoment = moment(currentTimePeriod.start);
                startDate = startMoment.format('YYYY-MM-DD');
                startTime = startMoment.format('HH:mm');
            }

            var dialogElement = $('<div class="time-period-dialog">'
                    + '<label>Date: <input type="date" class="time-period-date" value="' + startDate + '" /></label>'
                    + '<label>Time: <input type="time" class="time-period-time" value="' + startTime + '"/></label>'
                    + '<label>Period: <select class="time-period-picker">' + timePeriodSelect.html() + '</select>'
                    + '</label>' 
                + '</div>')
                .appendTo($('body'))
                .dialog({
                    title: 'Select Time Period',
                    resizable: false,
                    draggable: false,
                    modal: true,
                    close: closeDialog,
                    width: 400,
                    height: 240,
                    buttons: {
                        'Set': function () {
                            var startMoment = moment($('.time-period-date', dialogElement).val() + 'T' + $('.time-period-time', dialogElement).val() + ':00');

                            window.location = buildUrl(timePeriodPickerElement.children(':selected:first').data('timeLimit') + '&start=' + startMoment.format());
                        },
                        'Cancel': closeDialog
                    }
                });
            var timePeriodPickerElement = $('.time-period-picker', dialogElement);

            selectTimePeriod(timePeriodPickerElement, currentTimePeriod.timePeriod);

            function closeDialog() {
                dialogElement.dialog('close').remove();
            }
        }

        element
            .on('change', function () {
                var timeLimit = $(this).children(':selected:first').data('timeLimit');
                
                if (timeLimit) {
                    window.location = buildUrl(timeLimit);
                    return;
                }

                setCustomPeriod();
                selectTimePeriod(element);
            });

        selectTimePeriod(element);
    };
})();