(function () {
    TLRGRP.namespace('TLRGRP.BADGER.WMI');

    TLRGRP.BADGER.IIS = (function () {
        var iis = (function () {
            return (function () {
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('lr_web_request');
            });
        })();

        var pageTypes = TLRGRP.BADGER.Pages.allForArea('UserJourney');

        var statuses = [{
            id: 'NotFound',
            status: 404,
            title: '404 (Not Found)'
        }, {
            id: 'Error',
            status: 500,
            title: '500 (Error)'
        }, {
            id: 'Redirect',
            status: [301, 302],
            title: '30x (Redirect)'
        }];

        var channels = [{
            title: 'Direct',
            pagechannel: 'web'
        }, {
            title: 'Mobile',
            pagechannel: 'mobile'
        }, {
            title: 'Affiliate',
            pagechannel: 'affiliate'
        }];

        var allMetrics = {
            'AllTraffic': {
                title: 'All',
                expression: iis().sum(),
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'BotTraffic': {
                title: 'Bot',
                expression: iis().sum().equalTo('isbot', true),
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'MobileTraffic': {
                title: 'Mobile',
                expression: iis().sum().equalTo('isbot', false).equalTo('ismobile', true),
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'IPGResponseTime': {
                title: 'Average Time for Tokenisation',
                expression: iis().median('token_duration').matchesRegEx('url', '/beacon/tokeniserresponse'),
                chartOptions: {
                    yAxisLabel: 'duration (ms)'
                }
            }
        };

        function addRequestsMetrics() {
            function stripSpaces(input) {
                return input.replace(/\s/g, '');
            }

            function metricExpander(metricCollection, options) {
                var metrics = {};
                var property = options.property;

                _(metricCollection).each(function (metric) {
                    var metricId = options.titleFormatString.replace('{MetricId}', (metric.id || stripSpaces(metric.title)));
                    var reducer = typeof metric[property] === "object" && metric[property].join ? 'in' : 'equalTo';

                    metrics[metricId] = {
                        title: metric.title,
                        expression: iis()[options.aggregate](options.aggregateField)[reducer](property, metric[property]),
                        chartOptions: {
                            yAxisLabel: options.yAxisLabel
                        }
                    };
                });

                return metrics;
            }

            $.extend(true, allMetrics,
                metricExpander(statuses, {
                    property: 'status',
                    aggregate: 'sum',
                    titleFormatString: '{MetricId}Response',
                    yAxisLabel: 'requests'
                }),
                metricExpander(pageTypes, {
                    property: 'pagetype',
                    aggregate: 'sum',
                    titleFormatString: '{MetricId}Requests',
                    yAxisLabel: 'requests'
                }),
                metricExpander(channels, {
                    property: 'pagechannel',
                    aggregate: 'sum',
                    titleFormatString: 'Channel{MetricId}Requests',
                    yAxisLabel: 'requests'
                }),
                metricExpander(pageTypes, {
                    property: 'pagetype',
                    aggregate: 'median',
                    aggregateField: 'duration',
                    titleFormatString: '{MetricId}ServerResponseTime',
                    yAxisLabel: 'request time (ms)'
                }));
        };

        addRequestsMetrics();

        return {
            metricInfo: function (metricName) {
                return allMetrics[metricName];
            }
        };
    })();
})();