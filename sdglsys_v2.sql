/*
Navicat MariaDB Data Transfer

Source Server         : localhost_3306
Source Server Version : 100109
Source Host           : localhost:3306
Source Database       : sdglsys

Target Server Type    : MariaDB
Target Server Version : 100109
File Encoding         : 65001

Date: 2018-09-27 17:31:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for t_bill
-- ----------------------------
DROP TABLE IF EXISTS `t_bill`;
CREATE TABLE `t_bill` (
  `bill_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '账单ID',
  `bill_used_id` int(11) DEFAULT NULL COMMENT '用量登记ID',
  `bill_quota_id` int(11) DEFAULT NULL COMMENT '基础配额ID',
  `bill_rates_id` int(11) DEFAULT NULL COMMENT '费率ID',
  `bill_dorm_id` int(11) DEFAULT NULL COMMENT '园区ID',
  `bill_building_id` int(11) DEFAULT NULL COMMENT '宿舍楼ID',
  `bill_room_id` int(11) DEFAULT NULL COMMENT '宿舍ID',
  `bill_cold_water_cost` decimal(8,2) DEFAULT NULL COMMENT '冷水费用',
  `bill_hot_water_cost` decimal(8,2) DEFAULT NULL COMMENT '热水费用',
  `bill_electric_cost` decimal(8,2) DEFAULT NULL COMMENT '电费',
  `bill_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `bill_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '生成时间',
  `bill_mod_date` datetime DEFAULT NULL COMMENT '修改时间',
  `bill_is_active` tinyint(2) DEFAULT '1' COMMENT '状态：0已注销，1已登记，2已结算',
  `bill_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`bill_id`),
  KEY `bill_used_id` (`bill_used_id`),
  KEY `bill_rates_id` (`bill_rates_id`),
  KEY `bill_dorm_id` (`bill_dorm_id`),
  KEY `bill_building_id` (`bill_building_id`),
  KEY `bill_room_id` (`bill_room_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='账单信息表';

-- ----------------------------
-- Table structure for t_building
-- ----------------------------
DROP TABLE IF EXISTS `t_building`;
CREATE TABLE `t_building` (
  `building_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '宿舍楼ID',
  `building_dorm_id` int(11) DEFAULT NULL COMMENT '园区ID',
  `building_vid` varchar(15) DEFAULT NULL COMMENT '宿舍楼编号',
  `building_nickname` varchar(20) NOT NULL COMMENT '宿舍楼名称',
  `building_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `building_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `building_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`building_id`),
  KEY `building_dorm_id` (`building_dorm_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='宿舍楼信息表';

-- ----------------------------
-- Table structure for t_dorm
-- ----------------------------
DROP TABLE IF EXISTS `t_dorm`;
CREATE TABLE `t_dorm` (
  `dorm_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '园区ID',
  `dorm_nickname` varchar(20) NOT NULL COMMENT '园区名称',
  `dorm_type` tinyint(1) DEFAULT '1' COMMENT '园区类型：0女，1男，默认1',
  `dorm_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `dorm_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `dorm_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`dorm_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='园区信息表';

-- ----------------------------
-- Table structure for t_log
-- ----------------------------
DROP TABLE IF EXISTS `t_log`;
CREATE TABLE `t_log` (
  `log_id` int(11) NOT NULL AUTO_INCREMENT,
  `log_login_name` varchar(15) DEFAULT NULL COMMENT '用户名',
  `log_ip` varchar(46) DEFAULT NULL COMMENT '操作IP',
  `log_info` text COMMENT '日志信息',
  `log_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '发生时间',
  PRIMARY KEY (`log_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='日志信息表'
 PARTITION BY RANGE (log_id)
(PARTITION p0 VALUES LESS THAN (500000) ENGINE = InnoDB,
 PARTITION p1 VALUES LESS THAN (1000000) ENGINE = InnoDB,
 PARTITION p2 VALUES LESS THAN (2000000) ENGINE = InnoDB,
 PARTITION p3 VALUES LESS THAN (4000000) ENGINE = InnoDB,
 PARTITION p4 VALUES LESS THAN MAXVALUE ENGINE = InnoDB);


-- ----------------------------
-- Table structure for t_notice
-- ----------------------------
DROP TABLE IF EXISTS `t_notice`;
CREATE TABLE `t_notice` (
  `notice_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '公告ID',
  `notice_login_name` varchar(15) DEFAULT NULL COMMENT '发布者用户名',
  `notice_title` varchar(40) DEFAULT NULL COMMENT '标题',
  `notice_content` text COMMENT '内容（经过ZIP压缩的HTML文档）',
  `notice_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '发布时间',
  `notice_mod_date` datetime DEFAULT NULL COMMENT '修改时间',
  `notice_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `notice_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`notice_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='公告信息表';

-- ----------------------------
-- Table structure for t_quota
-- ----------------------------
DROP TABLE IF EXISTS `t_quota`;
CREATE TABLE `t_quota` (
  `quota_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '基础配额ID',
  `quota_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间',
  `quota_cold_water_value` float DEFAULT NULL COMMENT '冷水配额',
  `quota_hot_water_value` float DEFAULT NULL COMMENT '热水配额',
  `quota_electric_value` float DEFAULT NULL COMMENT '电力配额',
  `quota_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `quota_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `quota_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`quota_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='基础配额信息表';

-- ----------------------------
-- Table structure for t_rate
-- ----------------------------
DROP TABLE IF EXISTS `t_rate`;
CREATE TABLE `t_rate` (
  `rate_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '费率ID',
  `rate_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '更新时间',
  `rate_cold_water_value` float DEFAULT NULL COMMENT '冷水费率',
  `rate_hot_water_value` float DEFAULT NULL COMMENT '热水费率',
  `rate_electric_value` float DEFAULT NULL COMMENT '电力费率',
  `rate_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `rate_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `rate_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`rate_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='费率信息表';

-- ----------------------------
-- Table structure for t_room
-- ----------------------------
DROP TABLE IF EXISTS `t_room`;
CREATE TABLE `t_room` (
  `room_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '宿舍ID',
  `room_building_id` int(11) NOT NULL COMMENT '宿舍楼ID',
  `room_dorm_id` int(11) NOT NULL COMMENT '园区ID',
  `room_vid` varchar(15) NOT NULL COMMENT '宿舍编号',
  `room_nickname` varchar(20) NOT NULL COMMENT '宿舍名称',
  `room_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `room_is_active` tinyint(1) NOT NULL COMMENT '状态：1激活，0注销',
  `room_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  `number` tinyint(2) NOT NULL COMMENT '宿舍人数',
  PRIMARY KEY (`room_id`),
  KEY `room_building_id` (`room_building_id`),
  KEY `room_dorm_id` (`room_dorm_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COMMENT='宿舍信息表';

-- ----------------------------
-- Table structure for t_used
-- ----------------------------
DROP TABLE IF EXISTS `t_used`;
CREATE TABLE `t_used` (
  `used_id` int(11) NOT NULL AUTO_INCREMENT,
  `used_room_id` int(11) DEFAULT NULL COMMENT '宿舍ID',
  `used_dorm_id` int(11) NOT NULL COMMENT '园区ID',
  `used_building_id` int(11) NOT NULL COMMENT '宿舍楼ID',
  `used_post_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '登记时间',
  `used_post_user_id` int(11) DEFAULT NULL COMMENT '登记者用户ID',
  `used_cold_water_value` float DEFAULT NULL COMMENT '冷水用量',
  `used_hot_water_value` float DEFAULT NULL COMMENT '热水用量',
  `used_electric_value` float DEFAULT NULL COMMENT '用电量',
  `used_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `used_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `used_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`used_id`),
  KEY `used_room_id` (`used_room_id`),
  KEY `used_dorm_id` (`used_dorm_id`),
  KEY `used_building_id` (`used_building_id`),
  KEY `used_post_user_id` (`used_post_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用量登记表';

-- ----------------------------
-- Table structure for t_used_total
-- ----------------------------
DROP TABLE IF EXISTS `t_used_total`;
CREATE TABLE `t_used_total` (
  `ut_id` int(11) NOT NULL AUTO_INCREMENT,
  `ut_room_id` int(11) DEFAULT NULL COMMENT '宿舍ID',
  `ut_dorm_id` int(11) NOT NULL COMMENT '园区ID',
  `ut_building_id` int(11) NOT NULL COMMENT '宿舍楼ID',
  `ut_post_date` datetime DEFAULT NULL COMMENT '更新时间',
  `ut_cold_water_value` float DEFAULT NULL COMMENT '冷水表读数',
  `ut_hot_water_value` float DEFAULT NULL COMMENT '热水表读数',
  `ut_electric_value` float DEFAULT NULL COMMENT '电表读数',
  `ut_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `ut_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`ut_id`),
  KEY `ut_room_id` (`ut_room_id`),
  KEY `ut_dorm_id` (`ut_dorm_id`),
  KEY `ut_building_id` (`ut_building_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='宿舍水电表读数表';

-- ----------------------------
-- Table structure for t_user
-- ----------------------------
DROP TABLE IF EXISTS `t_user`;
CREATE TABLE `t_user` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `user_login_name` varchar(15) NOT NULL COMMENT '用户名',
  `user_nickname` varchar(15) NOT NULL COMMENT '姓名',
  `user_pwd` varchar(64) NOT NULL COMMENT '密码',
  `user_note` varchar(255) DEFAULT NULL COMMENT '备注',
  `user_phone` varchar(11) DEFAULT NULL COMMENT '联系电话',
  `user_dorm_id` int(11) DEFAULT NULL COMMENT '所属园区ID',
  `user_role` int(11) NOT NULL COMMENT '权限：1辅助登记员，2宿舍管理员，3系统管理员',
  `user_reg_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `user_is_active` tinyint(1) DEFAULT '1' COMMENT '状态：1激活，0注销',
  `user_model_state` tinyint(1) DEFAULT '1' COMMENT '对象状态：1正常，0已删除',
  PRIMARY KEY (`user_id`,`user_login_name`),
  UNIQUE KEY `user_login_name` (`user_login_name`)
) ENGINE=InnoDB AUTO_INCREMENT=1000 DEFAULT CHARSET=utf8 COMMENT='系统角色信息表';

-- ----------------------------
-- Table structure for t_token
-- ----------------------------
DROP TABLE IF EXISTS `t_token`;
CREATE TABLE `t_token` (
  `token_id` varchar(36) NOT NULL COMMENT 'Token ID',
  `token_user_id` int(11) NOT NULL COMMENT '用户ID',
  `token_login_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '登录时间',
  `token_expired_date` datetime DEFAULT CURRENT_TIMESTAMP COMMENT 'Token过期时间，默认1个月',
  PRIMARY KEY (`token_id`,`token_user_id`),
  UNIQUE KEY `token_id` (`token_id`),
  UNIQUE KEY `token_user_id` (`token_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Token验证表';