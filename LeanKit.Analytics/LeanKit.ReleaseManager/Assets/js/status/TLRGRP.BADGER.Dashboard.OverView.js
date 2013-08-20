(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    TLRGRP.BADGER.Dashboard.Overview = function () {
        var currentStep = '';
        var currentLimit = '';

        return {
            toString: function () {
                return 'Overview';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'Overview': {}
                });
            },
            supportsView: function (view) {
                return view === 'Overview';
            },
            getView: function () {
                return '';
            },
            setView: function () { },
            setTimePeriod: function () {
            },
            getGraphs: function (currentTimitSelectDataString) {
                var graphs = [{
                    title: 'Traffic by Type',
                    'class': 'half',
                    expressions: [{
                        id: 'iis-all',
                        title: 'All',
                        color: colors[0],
                        expression: 'sum(lr_web_request)&' + currentTimitSelectDataString
                    },
                        {
                            id: 'iis-bot',
                            title: 'Bot',
                            color: colors[1],
                            expression: 'sum(lr_web_request.eq(isbot,true))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'iis-mobile',
                            title: 'Mobile',
                            color: colors[4],
                            expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile,true))&' + currentTimitSelectDataString
                        }],
                    chartOptions: {
                        yAxisLabel: 'requests',
                        dimensions: {
                            margin: { left: 50 }
                        }
                    }
                },
                    {
                        title: 'Traffic by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: colors[0],
                            expression: 'sum(lr_web_request.eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                        },
                            {
                                id: 'page-requests-search',
                                title: 'Search',
                                color: colors[2],
                                expression: 'sum(lr_web_request.eq(pagetype,"search"))&' + currentTimitSelectDataString
                            },
                            {
                                id: 'page-requests-hotel-details',
                                title: 'Hotel Details',
                                color: colors[4],
                                expression: 'sum(lr_web_request.eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                            }],
                        chartOptions: {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        }
                    },
                    {
                        title: 'Traffic by Channel',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-web',
                            title: 'Direct',
                            color: colors[0],
                            expression: 'sum(lr_web_request.eq(pagechannel,"web"))&' + currentTimitSelectDataString
                        },
                            {
                                id: 'page-requests-mobile',
                                title: 'Mobile',
                                color: colors[2],
                                expression: 'sum(lr_web_request.eq(pagechannel,"mobile"))&' + currentTimitSelectDataString
                            },
                            {
                                id: 'page-requests-affiliate',
                                title: 'Affiliate',
                                color: colors[4],
                                expression: 'sum(lr_web_request.eq(pagechannel,"affiliate"))&' + currentTimitSelectDataString
                            }],
                        chartOptions: {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        }
                    }, {
                        title: 'Response Time by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: colors[0],
                            expression: 'median(lr_web_request(duration).eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-search',
                            title: 'Search',
                            color: colors[2],
                            expression: 'median(lr_web_request(duration).eq(pagetype,"search"))&' + currentTimitSelectDataString
                        },
                        {
                            id: 'page-requests-hotel-details',
                            title: 'Hotel Details',
                            color: colors[4],
                            expression: 'median(lr_web_request(duration).eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                        }],
                        chartOptions: {
                            yAxisLabel: 'request time (ms)',
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