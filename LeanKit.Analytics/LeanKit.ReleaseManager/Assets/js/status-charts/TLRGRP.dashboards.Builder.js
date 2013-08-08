TLRGRP.namespace('TLRGRP.dashboards');

(function () {
	TLRGRP.dashboards.Builder = function(element, config) {
		var buildGraph = function(item) {
				return $('<div />', {
						class: 'graph-container' + (item.class ? ' ' + item.class : '')
					})
					.append($('<div />', {
						class: 'graph-title',
						text: item.title
					}))
					.append($('<iframe />', {
						src: 'assets/graph.html?expressions=' + encodeURIComponent(JSON.stringify(item.expressions)) 
							+ '&chartOptions=' + encodeURIComponent(JSON.stringify(item.chartOptions)) ,
						frameborder: '0'
					}));
			};
			
		element.append($.map(config, buildGraph));
	};
})();