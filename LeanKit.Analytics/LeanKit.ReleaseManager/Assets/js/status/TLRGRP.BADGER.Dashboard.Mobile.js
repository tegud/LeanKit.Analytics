(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    TLRGRP.BADGER.Dashboard.Mobile = function () {
        var isSelected;
        var currentTimePeriod = '1hour';
        var chartOptions = {
            lockToZero: true
        };

        return {
            toString: function () {
                return 'Mobile';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'Mobile': {}
                });
            },
            appendViewModel: function (viewModel) {
                if (isSelected) {
                    $.extend(viewModel, {
                        pageName: 'Mobile',
                        timePeriod: currentTimePeriod
                    });
                }

                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'Mobile',
                    metric: 'Mobile',
                    isSelected: isSelected
                };
            },
            supportsView: function (view) {
                return view === 'Mobile';
            },
            setView: function () {
                isSelected = true;
            },
            clearView: function () {
                isSelected = false;
            },
            setTimePeriod: function (timePeriod) {
                currentTimePeriod = timePeriod;
            },
            getGraphs: function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                var graphs = [{
                    title: 'Mobile Traffic',
                    'class': 'half',
                    expressions: [{
                        id: 'mobile-on-mobile',
                        title: 'On Mobile',
                        color: colors[3],
                        graphType: 'stacked-area',
                        expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile, true).eq(pagechannel,"mobile"))&' + currentTimitSelectDataString
                    },
                        {
                            id: 'mobile-on-desktop',
                            title: 'On Desktop',
                            color: colors[2],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile, true).ne(pagechannel,"mobile"))&' + currentTimitSelectDataString
                        }],
                    chartOptions: $.extend({}, chartOptions, {
                        yAxisLabel: 'requests',
                        dimensions: {
                            margin: { left: 50 }
                        }
                    })
                },
                    {
                        title: 'Mobile Site traffic by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: colors[0],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(pagechannel,"mobile").eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-search',
                            title: 'Search',
                            color: colors[2],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(pagechannel,"mobile").eq(pagetype,"search"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-hotel-details',
                            title: 'Hotel Details',
                            color: colors[4],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(pagechannel,"mobile").eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        })
                    }, {
                        title: 'Response Time by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: colors[0],
                            expression: 'median(lr_web_request(duration).eq(pagechannel,"mobile").eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-search',
                            title: 'Search',
                            color: colors[2],
                            expression: 'median(lr_web_request(duration).eq(pagechannel,"mobile").eq(pagetype,"search"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-hotel-details',
                            title: 'Hotel Details',
                            color: colors[4],
                            expression: 'median(lr_web_request(duration).eq(pagechannel,"mobile").eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'request time (ms)',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        })
                    },
                    {
                        title: 'Mobile traffic on Desktop by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: colors[0],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile, true).ne(pagechannel,"mobile").eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-search',
                            title: 'Search',
                            color: colors[2],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile, true).ne(pagechannel,"mobile").eq(pagetype,"search"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-hotel-details',
                            title: 'Hotel Details',
                            color: colors[4],
                            graphType: 'stacked-area',
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile, true).ne(pagechannel,"mobile").eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        })
                    }];

                return graphs;
            }
        };
    };
})();