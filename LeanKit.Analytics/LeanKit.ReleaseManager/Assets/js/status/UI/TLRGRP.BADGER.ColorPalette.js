(function () {
    TLRGRP.namespace('TLRGRP.BADGER.ColorPalette');

    TLRGRP.BADGER.ColorPalette = function () {
        var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

        var presetColors = {
            homepage: colors[0],
            search: colors[2],
            bookingpage: colors[3],
            hoteldetails: colors[4],
            mobile: colors[2],
            direct: colors[0],
            affiliate: colors[4],
            error: colors[1]
        };

        function getColorByIndex(i) {
            return colors[i];
        }

        function getColorByKey(expressionKey, i) {
            if (expressionKey.color) {
                return expressionKey.color;
            }

            var lowerCaseId = expressionKey.id.toLowerCase();

            for (var preset in presetColors) {
                if (lowerCaseId.indexOf(preset) > -1) {
                    return presetColors[preset];
                }
            }

            return getColorByIndex(i);
        }

        return {
            getColorByKey: getColorByKey,
            getColorByIndex: getColorByIndex
        };
    };
})();