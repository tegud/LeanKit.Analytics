(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Machines');

    TLRGRP.BADGER.Machines = (function () {
        var webPrefix = 'TELWEB';
        var serverRanges = {
            'web': [1, 19],
            'ssl': [107, 109]
        };

        function buildServerName(machineNumber) {
            var machineNumberLength = (machineNumber + '').length;
            var id = machineNumber;

            for (var x = machineNumberLength; x < 3; x++) {
                id = '0' + id;
            }

            return webPrefix + id + 'P';
        }
        
        function appendServers(range, allServers) {
            var x = serverRanges[range][0];
            
            for (; x <= serverRanges[range][1]; x++) {
                allServers[allServers.length] = buildServerName(x);
            }
        }

        return {
            getFullNameForWebServer: function (machineNumber) {
                return buildServerName(machineNumber);
            },
            getAllServers: function() {
                var allServers = [];

                for (range in serverRanges) {
                    if (!serverRanges.hasOwnProperty(range)) {
                        continue;
                    }
                    
                    appendServers(range, allServers);
                }

                return allServers;
            },
            getServerRange: function(range) {
                var allServers = [];

                appendServers(range, allServers);

                return allServers;
            }
        };
    })();
})();