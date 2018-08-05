layui.config({
	base: '/Content/js/module/'
}).extend({
/*	dialog: 'dialog',*/
});

function reloadPage() {
	setTimeout('window.location.reload()', 1200); // 延时刷新页面
}

layui.use(['form', 'jquery', 'laydate', 'layer', 'laypage', 'dialog', 'element'], function() {
	var form = layui.form,
		layer = layui.layer,
		$ = layui.jquery,
		dialog = layui.dialog;
	//获取当前iframe的name值
	var iframeObj = $(window.frameElement).attr('name');
	//全选
	form.on('checkbox(allChoose)', function(data) {
		var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
		child.each(function(index, item) {
			item.checked = data.elem.checked;
		});
		form.render('checkbox');
	});
	//渲染表单
	form.render();
	//顶部添加
    $('.addBtn').click(function () {
        var url = $(this).attr('data-url');
        var title = $(this).attr('data-text');
        //将iframeObj传递给父级窗口,执行操作完成刷新
        parent.page(title, url, iframeObj, w = "700px", h = "620px");
        return false;
    }).mouseenter(function () {
        dialog.tips('添加', '.addBtn');
        });
    // 顶部刷新 sinsen
    $('.reloadBtn').click(function () {
        location.reload();
    }).mouseenter(function () {
        dialog.tips('刷新', '.reloadBtn');
    });
	//顶部排序
	$('.listOrderBtn').click(function() {
		var url = $(this).attr('data-url');
		dialog.confirm({
			message: '您确定要进行排序吗？',
			success: function() {
				layer.msg('确定了')
			},
			cancel: function() {
				layer.msg('取消了')
			}
		})
		return false;

	}).mouseenter(function() {

		dialog.tips('批量排序', '.listOrderBtn');

	})
	//顶部批量删除
	$('.delBtn').click(function() {
		var url = $(this).attr('data-url');
		dialog.confirm({
			message: '您确定要删除选中项',
			success: function() {
				layer.msg('删除了')
			},
			cancel: function() {
				layer.msg('取消了')
			}
		})
		return false;

	}).mouseenter(function() {

		dialog.tips('批量删除', '.delBtn');

	})
	//列表查看详情
	$('#table-list').on('click', '.view-btn', function() {
		var content = $(this).attr('title');
		var title = '查看详情';
		//将iframeObj传递给父级窗口
		layer.open({
			'title': title,
            'content': content.replace('|', '<br/>').replace('|', '<br/>').replace('|', '<br/>').replace('|', '<br/>') // 自动替换英文里|为换行
		})
	})
	//列表添加
	$('#table-list').on('click', '.add-btn', function() {
		var url = $(this).attr('data-url');
		var title = $(this).attr('data-text');
		//将iframeObj传递给父级窗口
		parent.page(title, url, iframeObj, w = "700px", h = "620px");
	})
	//列表删除
	$('#table-list').on('click', '.del-btn', function() {
		var url = $(this).attr('data-url');
		var id = $(this).attr('data-id');
		var params = JSON.stringify({
			"type": "del",
			"id": id,
		});

		layer.confirm('将会同时删除相关联数据', {
			icon: 10,
            title: '您确定要进行删除吗？'
        }, function (index) {
            $.ajax({
                async: "false",
                type: "post",
                //dataType: "json",
                //contentType: "application/json",
                data: (params),
                url: url,
                success: function (data) {
                    data = JSON.parse(data);
                    layer.msg(data['msg'], {
                        'icon': data['code'] == 200 ? 1 : 0,
                    });
                    reloadPage();
                },
                error: function (e) {
                    layer.open({
                        'icon': 0,
                        'title': "发生错误",
                        'content': "错误代码：" + e['status'] + "<br/>错误提示信息：" + e["statusText"]
                    });
                }
            });
		});
	})
	
	//删除用量登记
	$('#table-list').on('click', '.del-used', function() {
		var url = $(this).attr('data-url');
		var id = $(this).attr('data-id');
		var params = JSON.stringify({
			"type": "del",
            'id': id
        });
		layer.confirm('将会同时删除账单记录！', {
			icon: 10,
			title: '您确定要进行删除吗？'
        }, function (index) {
            $.ajax({
                async: "false",
                type: "post",
                //dataType: "json",
                //contentType: "application/json",
                data: (params),
                url: url,
                success: function (data) {
                    data = JSON.parse(data);
                    layer.msg(data['msg'], {
                        'icon': data['code'] == 200 ? 1 : 0,
                    });
                    reloadPage();
                },
                error: function (e) {
                    layer.open({
                        'icon': 0,
                        'title': "发生错误",
                        'content': "错误代码：" + e['status'] + "<br/>错误提示信息：" + e["statusText"]
                    });
                }
            });
		});
	})

    //删除宿舍读表信息
    $('#table-list').on('click', '.del-usedinfo', function () {
        var url = $(this).attr('data-url');
        var id = $(this).attr('data-id');
        layer.confirm('您确定要进行删除吗？', {
            icon: 10,
            title: '温馨提示'
        }, function (index) {
            $.ajax({
                async: "false",
                type: "post",
                //dataType: "json",
                //contentType: "application/json",
                data: {},
                url: url,
                success: function (data) {
                    data = JSON.parse(data);
                    layer.msg(data['msg'], {
                        'icon': data['code'] == 200 ? 1 : 0,
                    });
                    reloadPage();
                },
                error: function (e) {
                    layer.open({
                        'icon': 0,
                        'title': "发生错误",
                        'content': "错误代码：" + e['status'] + "<br/>错误提示信息：" + e["statusText"]
                    });
                }
            });
        });
    })

	//缴费操作
	$('#table-list').on('click', '.pay-btn', function() {
		var url = $(this).attr('data-url');
		var id = $(this).attr('data-id');
		var params = JSON.stringify({
            "type": "changeStat",
			'id': id,
			'is_active': 2
		});
		layer.confirm('确认把该账单设置成已缴费状态？', {
			icon: 10,
            title: '结算账单'
        }, function (index) {
            $.ajax({
                async: "false",
                type: "post",
                //dataType: "json",
                //contentType: "application/json",
                data: (params),
                url: url,
                success: function (data) {
                    data = JSON.parse(data);
                    layer.msg(data['msg'], {
                        'icon': data['code'] == 200 ? 1 : 0,
                    });
                    reloadPage();
                },
                error: function (e) {
                    layer.open({
                        'icon': 0,
                        'title': "发生错误",
                        'content': "错误代码：" + e['status'] + "<br/>错误提示信息：" + e["statusText"]
                    });
                }
            });
		});
	})
	//列表跳转
	$('#table-list,.tool-btn').on('click', '.go-btn', function() {
		var url = $(this).attr('data-url');
		var id = $(this).attr('data-id');
		window.location.href = url + "?id=" + id;
		return false;
	})
	//编辑栏目
	$('#table-list').on('click', '.edit-btn', function() {
		var That = $(this);
		var id = $(this).attr('data-id');
		var url = That.attr('data-url');
		var title = That.attr('data-text') ? That.attr('data-text') : "编辑";
		//将iframeObj传递给父级窗口
		parent.page(title, url + "?id=" + id, iframeObj, w = "700px", h = "620px");
	});
});

/**
 * 控制iframe窗口的刷新操作
 */
var iframeObjName;

//父级弹出页面
function page(title, url, obj, w, h) {
	if(title == null || title == '') {
		title = false;
	};
	if(url == null || url == '') {
		url = "404.html";
	};
	if(w == null || w == '') {
		w = '700px';
	};
	if(h == null || h == '') {
		h = '350px';
	};
	iframeObjName = obj;
	//如果手机端，全屏显示
	if(window.innerWidth <= 768) {
		var index = layer.open({
			type: 2,
			title: title,
			area: [320, h],
			fixed: false, //不固定
			content: url
		});
		layer.full(index);
	} else {
		var index = layer.open({
			type: 2,
			title: title,
			area: [w, h],
			fixed: true, //不固定
			content: url
		});
	}
}

/**
 * 刷新子页,关闭弹窗
 */
function refresh() {
	//根据传递的name值，获取子iframe窗口，执行刷新
	if(window.frames[iframeObjName]) {
		window.frames[iframeObjName].location.reload();

	} else {
		window.location.reload();
	}

	layer.closeAll();
}