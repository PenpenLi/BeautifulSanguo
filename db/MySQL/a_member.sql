/*
Navicat MySQL Data Transfer

Source Server         : Sanguo
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sanguo

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-09 08:38:19
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `a_member`
-- ----------------------------
DROP TABLE IF EXISTS `a_member`;
CREATE TABLE `a_member` (
  `PlayerID` int(11) NOT NULL AUTO_INCREMENT COMMENT '玩家編號',
  `PlayerName` varchar(32) CHARACTER SET utf8 NOT NULL DEFAULT '' COMMENT '玩家姓名',
  `Money` int(11) NOT NULL DEFAULT '0' COMMENT '遊戲幣',
  `Coin` int(11) NOT NULL DEFAULT '0' COMMENT '商城幣',
  `LV` int(11) NOT NULL DEFAULT '1',
  `Exp` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`PlayerID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=big5;

-- ----------------------------
-- Records of a_member
-- ----------------------------
INSERT INTO `a_member` VALUES ('1', 'dandan', '0', '0', '1', '0');
