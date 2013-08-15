(function() {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    TLRGRP.BADGER.Dashboard.Overview = function () {
        return {
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'Overview': { }
                });
            },
            getGraphs: function (selectedView, currentStep, currentLimit) {
                var currentTimitSelectDataString = 'step=' + currentStep + '&limit=' + currentLimit;
                var graphs = [{
                    title: 'Traffic by Type',
                    'class': 'half',
                    expressions: [{
                        id: 'iis-all',
                        title: 'All Requests',
                        color: colors[0],
                        expression: 'sum(lr_web_request)&' + currentTimitSelectDataString
                    },
                        {
                            id: 'iis-bot',
                            title: 'Bot Requests',
                            color: colors[1],
                            expression: 'sum(lr_web_request.eq(isbot,true))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'iis-mobile',
                            title: 'Mobile Requests',
                            color: colors[4],
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile,true))&' + currentTimitSelectDataString
                        }],
                    chartOptions: {
                        dimensions: {
                            margin: { left: 50 }
                        }
                    }
                }, {
                    title: 'IIS Traffic',
                    'class': 'half',
                    expressions: [{
                        id: 'iis-base-all',
                        title: 'All Requests',
                        color: colors[0],
                        expression: 'sum(lr_web_iis)&' + currentTimitSelectDataString
                    }],
                    chartOptions: {
                        dimensions: {
                            margin: { left: 50 }
                        }
                    }
                }];
                
                return graphs;
            }
        };
    };
})();