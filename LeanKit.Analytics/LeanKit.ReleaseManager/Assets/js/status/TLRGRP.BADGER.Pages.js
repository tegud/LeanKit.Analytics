(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Pages');

    TLRGRP.BADGER.Pages = (function () {
        var areas = {};

        function findPageInArea(area, page) {
            return areas[area][page];
        }
        
        function findPageInAnyArea(page) {
            var match;

            for (area in areas) {
                if (!areas.hasOwnProperty(area)) {
                    continue;
                }

                if (areas[area][page]) {
                    match = areas[area][page];
                    break;
                }
            }
            return match;
        }

        function normalisePage(page) {
            page.name = page.id;

            return page;
        }

        function registerPageWithArea(area, page) {
            if (!areas[area]) {
                areas[area] = {};
            }

            areas[area][page.id] = normalisePage(page);
        }

        return {
            get: function () {
                var area;
                if (arguments[1]) {
                    area = arguments[0];
                }
                var page = !area ? arguments[0] : arguments[1];

                if (area) {
                    return findPageInArea(area, page);
                }

                return findPageInAnyArea(page);
            },
            allForArea: function(area) {
                return areas[area] || {};
            },
            register: function () {
                var area;
                
                if (typeof arguments[0] === 'string') {
                    area = arguments[0];
                }

                _(arguments).each(function (page) {
                    if (typeof page === 'string') {
                        return true;
                    }
                    
                    var currentArea = page.area || area || 'Unknown';

                    registerPageWithArea(currentArea, page);
                });
            }
        };
    })();
})();