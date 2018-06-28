//图表
/* Sinssen
 * 2018年6月9日
 * 分割为独立js文件
 */
// 数值显示
var labelOption = {
	show: true,
	formatter: '{c}',
	fontSize: 14,
	offset: [0, -14],
	rich: {
		name: {
			textBorderColor: ''
		}
	},
	color: "rgb(100,100,100)"
};
//--- 默认显示的图表 ---
var myChart = echarts.init(document.getElementById('chart'));

function ChartLoadData(data) {
	if(data.length<1)return;
	myChart.setOption({
		title: {
			text: "用量登记统计表",
			x: 'center',
			y: 'bottom',
			textStyle: {
				color: "rgb(85, 85, 85)",
				fontSize: 14,
			}
		},
		legend: {},
		tooltip: {
			trigger: "axis"
		},
		toolbox: {
			top: '20px',
			show: true,
			feature: {
				magicType: {
					show: true,
					type: ["line", "bar", "stack", "tiled"]
				},
				restore: {
					show: true
				},
				saveAsImage: {
					show: true
				}
			}
		},
		xAxis: [{
			type: "category",
			data:data['date'],
		}],
		yAxis: [{
			type: "value"
		}],
		series: [ /*
		           *放在前面的数据图表可能会被遮住，应该按照从小到大放置数据 
		           */
			{
				name: "冷水",
				type: 'line',
				smooth: true,
				itemStyle: {
					normal: {
						areaStyle: {
							type: "default"
						},
						color: '#00bfff',
					}
				},
				label: labelOption,
				data: data['cold_water_value'],
				color: '#00bfff',
			},
			{
				name: "用电",
				type: 'line',
				smooth: true,
				itemStyle: {
					normal: {
						areaStyle: {
							type: "default"
						},
						color: "rgb(110, 211, 199)"
					}
				},
				label: labelOption,
				data: data['electric_value']
			},{
				name: "热水",
				type: 'line',
				smooth: true,
				itemStyle: {
					normal: {
						areaStyle: {
							type: "default"
						},
						color: "#ff1a1a",
					},
					/*label: labelOption,*/
				},
				color: "#ff1a1a",
				label: labelOption,
				data: data['hot_water_value']
			}
		]
	});
}