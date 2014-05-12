/*
Navicat MySQL Data Transfer

Source Server         : Sanguo
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sanguo

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-09 08:38:10
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `a_account_stop_id`
-- ----------------------------
DROP TABLE IF EXISTS `a_account_stop_id`;
CREATE TABLE `a_account_stop_id` (
  `StopID` int(11) NOT NULL,
  `StopName` varchar(30) NOT NULL,
  PRIMARY KEY (`StopID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of a_account_stop_id
-- ----------------------------
INSERT INTO `a_account_stop_id` VALUES ('0', '正常');
INSERT INTO `a_account_stop_id` VALUES ('1', '無聊想停就停');
