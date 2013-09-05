(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Cube');

    var STEP = {
        TenSeconds: '1e4',
        OneMinute: '6e4',
        FiveMinutes: '3e5',
        OneHour: '36e5',
        OneDay: '864e5',
    };

    var timePeriodMappings = {
        '5mins': { step: STEP.TenSeconds, limit: 30, minutes: 5 },
        '15mins': { step: STEP.OneMinute, limit: 15, minutes: 15 },
        '30mins': { step: STEP.OneMinute, limit: 30, minutes: 30 },
        '1hour': { step: STEP.OneMinute, limit: 60, minutes: 60 },
        '4hours': { step: STEP.FiveMinutes, limit: 48, minutes: 240 },
        '12hours': { step: STEP.OneHour, limit: 12, minutes: 720 },
        '1day': { step: STEP.OneHour, limit: 24, minutes: 1440 }
    };

    TLRGRP.BADGER.Cube.convertTimePeriod = function (timePeriod) {
        if (typeof(timePeriod) === 'string') {
            timePeriod = {
                timePeriod: timePeriod
            };
        }

        var timePeriodMapping = timePeriodMappings[timePeriod.timePeriod];
        var timeString = 'step=' + timePeriodMapping.step;

        if (timePeriod.start) {
            var startMoment = moment(timePeriod.start);
            var endMoment = startMoment.clone().add('minutes', timePeriodMapping.minutes);

            var start = startMoment.utc().format('YYYY-MM-DDTHH:mm:ss') + 'Z';
            var stop = endMoment.utc().format('YYYY-MM-DDTHH:mm:ss') + 'Z';

            timeString += '&start=' + start + '&stop=' + stop;
        }
        else {
            timeString += '&limit=' + timePeriodMapping.limit;
        }

        return timeString;
    };

    TLRGRP.BADGER.Cube.ExpressionBuilder = function (metric) {
        var criteriaFunction = function (operator) {
            return function (key, value) {
                expressionCriteria[expressionCriteria.length] = {
                    operator: operator,
                    key: key,
                    value: value
                };

                return filterReturnObject;
            };
        };
        var reducerSetter = function (reducer) {
            return function (value) {
                expressionReducer = reducer;
                expressionValue = value;

                return filterReturnObject;
            };
        };
        var filterReturnObject = {
            equalTo: criteriaFunction('eq'),
            notEqualTo: criteriaFunction('ne'),
            matchesRegEx: criteriaFunction('re'),
            'in': criteriaFunction('in'),
            sum: reducerSetter('sum'),
            median: reducerSetter('median'),
            setTimePeriod: function (timePeriod) {
                expressionTimeSelection = timePeriod;

                return filterReturnObject;
            },
            build: function () {
                var expression = expressionReducer + '(' + metric;

                if (expressionValue) {
                    expression += '(' + expressionValue + ')';
                }

                if (expressionCriteria.length) {
                    expression += '.' + _.map(expressionCriteria, function (criteria) {
                        var value = criteria.value;

                        if (typeof value === 'string') {
                            value = '"' + value + '"';
                        }
                        if (typeof value === 'object' && value.join) {
                            value = '[' + value.join(',') + ']';
                        }

                        return criteria.operator + '(' + criteria.key + ',' + value + ')';
                    }).join('.');
                }

                return expression + ')&' + expressionTimeSelection;
            }
        };
        var expressionCriteria = [];
        var expressionValue;
        var expressionReducer;
        var expressionTimeSelection;

        return filterReturnObject;
    }

    TLRGRP.BADGER.Cube.WMI = (function () {
        return {
            buildExpression: function (selectedView, machineName, stepAndLimit) {
                var metric = selectedView.metric;
                var metricGroup = selectedView.group;
                var eventType = selectedView.eventType;
                var divideBy = selectedView.divideBy;

                return ['median(' + eventType + '(' + metric + ')',
                    '.eq(source_host,"' + machineName + '")',
                    '.eq(metricGroup,"' + metricGroup + '"))' + (divideBy || '') + '&',
                    stepAndLimit].join('');
            }
        };
    })();

    TLRGRP.BADGER.Cube.IIS = (function () {
        return {
            buildExpression: function (selectedView, machineName, stepAndLimit) {
                var expression = selectedView.expression;
                var divideBy = selectedView.divideBy;

                return expression + (divideBy || '') + '&' + stepAndLimit;
            }
        };
    })();
})();