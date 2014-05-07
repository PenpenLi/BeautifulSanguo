/*
Navicat MySQL Data Transfer

Source Server         : Sanguo
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : sanguo

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2014-05-07 08:30:21
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `l_webmethod`
-- ----------------------------
DROP TABLE IF EXISTS `l_webmethod`;
CREATE TABLE `l_webmethod` (
  `MethodLogID` int(11) NOT NULL AUTO_INCREMENT,
  `ActionDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `PID` int(11) NOT NULL DEFAULT '0',
  `MethodName` varchar(255) NOT NULL,
  `Args` varchar(255) NOT NULL,
  PRIMARY KEY (`MethodLogID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of l_webmethod
-- ----------------------------
INSERT INTO `l_webmethod` VALUES ('1', '2014-05-04 15:12:50', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('2', '2014-05-04 15:12:50', '1', 'Account_Check No Account', '{\"Result\":\"Check_Account_No_Account\"}');
INSERT INTO `l_webmethod` VALUES ('3', '2014-05-04 15:16:12', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('4', '2014-05-04 15:16:12', '3', 'Account_Check Success', '{\"Result\":\"Success\",\"AccountID\":2}');
INSERT INTO `l_webmethod` VALUES ('5', '2014-05-04 15:17:30', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('6', '2014-05-04 15:17:30', '5', 'Account_Check Success', '{\"Result\":\"Success\",\"AccountID\":2}');
INSERT INTO `l_webmethod` VALUES ('7', '2014-05-04 16:50:28', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('8', '2014-05-04 16:50:28', '7', 'Account_Check Success', '{\"Result\":\"Success\",\"AccountID\":2}');
INSERT INTO `l_webmethod` VALUES ('9', '2014-05-04 16:51:45', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('10', '2014-05-04 16:51:45', '9', 'Account_Check Success', '{\"Result\":\"Success\",\"AccountID\":2,\"SessionKey\":\"SessionKey:dandan\"}');
INSERT INTO `l_webmethod` VALUES ('11', '2014-05-04 17:58:08', '0', 'Account_CreatePlayer', '{\"SessionKey\":\"SessionKey:dandan\"}');
INSERT INTO `l_webmethod` VALUES ('12', '2014-05-04 17:58:27', '0', 'Account_CreatePlayer', '{\"SessionKey\":\"SessionKey:dandan\"}');
INSERT INTO `l_webmethod` VALUES ('13', '2014-05-04 17:59:39', '0', 'Account_CreatePlayer', '{\"SessionKey\":\"SessionKey:dandan\"}');
INSERT INTO `l_webmethod` VALUES ('14', '2014-05-06 21:20:52', '0', 'Account_Check', '{\"Account\":\"dandan\",\"Password\":\"silveran\"}');
INSERT INTO `l_webmethod` VALUES ('15', '2014-05-06 21:20:52', '14', 'Account_Check Success', '{\"Result\":\"Success\",\"AccountID\":2,\"PlayerID\":0,\"SessionKey\":\"SessionKey:dandan\",\"ClientAction\":[\"[ToNewPlayer, ]\"]}');
INSERT INTO `l_webmethod` VALUES ('16', '2014-05-06 21:56:28', '0', 'Account_CreatePlayer', '{\"SessionKey\":\"SessionKey:dandan\",\"PlayerName\":\"dandan\"}');
INSERT INTO `l_webmethod` VALUES ('17', '2014-05-06 21:56:32', '16', 'Account_CreatePlayer Success', '{\"PlayerName\":\"dandan\",\"PlayerID\":1}');
