namespace SkyCar.Coeus.Common.Message
{
    /// <summary>
    /// 消息编码定义
    /// </summary>
    public enum MsgCode
    {
        #region 错误消息

        /// <summary>
        /// {0}
        /// </summary>
        E_0000,
        /// <summary>
        /// {0}不能为空！
        /// </summary>
        E_0001,
        /// <summary>
        /// 请输入有效的{0}！
        /// </summary>
        E_0002,
        /// <summary>
        /// {0}必须在{1}到{2}之间！
        /// </summary>
        E_0003,
        /// <summary>
        /// {0}必须大于{1}！
        /// </summary>
        E_0004,
        /// <summary>
        /// {0}必须小于{1}！
        /// </summary>
        E_0005,
        /// <summary>
        /// {0}已存在，不能重复添加！
        /// </summary>
        E_0006,
        /// <summary>
        /// 请输入{0}有效的{1}！
        /// </summary>
        E_0007,
        /// <summary>
        /// {0}为必选项！
        /// </summary>
        E_0008,
        /// <summary>
        /// {0}不可高于{1}！
        /// </summary>
        E_0009,
        /// <summary>
        /// {0}失败！
        /// </summary>
        E_0010,
        /// <summary>
        /// {0}错误！
        /// </summary>
        E_0011,
        /// <summary>
        /// {0}不一致！
        /// </summary>
        E_0012,
        /// <summary>
        /// 请选择{0}！
        /// </summary>
        E_0013,
        /// <summary>
        /// 请至少添加一条{0}信息！
        /// </summary>
        E_0014,
        /// <summary>
        /// 不能{0}其他组织创建的{1}！
        /// </summary>
        E_0015,
        /// <summary>
        /// {0}为空，{1}失败！
        /// </summary>
        E_0016,
        /// <summary>
        /// {0}，不能{1}！
        /// </summary>
        E_0017,
        /// <summary>
        /// {0}失败，失败原因：{1}
        /// </summary>
        E_0018,
        /// <summary>
        /// 调用平台接口结果为空，{0}失败
        /// </summary>
        E_0019,
        /// <summary>
        /// 从平台获取{0}为空，{1}失败
        /// </summary>
        E_0020,
        /// <summary>
        /// 请至少填入{0}中的一项！
        /// </summary>
        E_0021,
        /// <summary>
        /// 条形码为：{0}，批次号为：{1}的配件已被使用，不能{2}！
        /// </summary>
        E_0022,
        /// <summary>
        /// {0}对应的{1}已审核，不能{2}！
        /// </summary>
        E_0023,
        /// <summary>
        /// 第{0}行，请正确输入{1}，{1}为{2}！
        /// </summary>
        E_0024,
        /// <summary>
        /// 汽修商户服务器信息获取失败！
        /// </summary>
        E_0025,
        /// <summary>
        /// 汽修商户服务器连接失败，请联系与您对接的云车工作人员
        /// </summary>
        E_0026,
        /// <summary>
        /// 第{0}行，请选择{1}！
        /// </summary>
        E_0027,
        /// <summary>
        /// 请正确输入{0}，{0}为{1}！
        /// </summary>
        E_0028,
        /// <summary>
        /// {0}字数不能超过{1}个
        /// </summary>
        E_0029,
        /// <summary>
        /// 配件：{0}（条形码：{1}）的库存{2}，{3}失败！
        /// </summary>
        E_0030,
        /// <summary>
        /// 调拨类型为库位转储，第{0}行，调入仓库要与调出仓库一致！
        /// </summary>
        E_0031,
        /// <summary>
        /// 请上传{0}图片
        /// </summary>
        E_0032,
        /// <summary>
        /// 配件：{0}的销售数量和出库数量的总和不一致，请调整销售数量或出库数量！
        /// </summary>
        E_0033,
        /// <summary>
        /// 请勾选有效的条码+批次号！
        /// </summary>
        E_0034,
        /// <summary>
        /// 打开条码打印机失败，请检查是否正常连接了打印机!
        /// </summary>
        E_0035,
        /// <summary>
        /// 第{0}行，请输入{1}！
        /// </summary>
        E_0036,
        /// <summary>
        /// 第{0}行，{1}与{2}不一致，请调整{3}！
        /// </summary>
        E_0037,
        /// <summary>
        /// 请至少勾选一条数据！
        /// </summary>
        E_0038,
        /// <summary>
        /// 客户：{0}已终止销售，不能创建{1}！
        /// </summary>
        E_0039,
        #endregion

        #region 提示消息

        /// <summary>
        /// {0}
        /// </summary>
        I_0000,
        /// <summary>
        /// {0}成功！
        /// </summary>
        I_0001,
        /// <summary>
        /// {0}失败！
        /// </summary>
        I_0002,
        /// <summary>
        /// 确定要{0}吗？
        /// </summary>
        I_0003,
        /// <summary>
        /// 当前入库单对应的[采购订单]存在[付款单]，是否使用其中一张作为当前入库单的结算依据？。
        /// </summary>
        I_0004,
        /// <summary>
        /// 发现需要多次打印的条码:\n\n
        /// </summary>
        I_0005,
        /// <summary>
        /// \n确定打印以上条码？
        /// </summary>
        I_0006,
        /// <summary>
        /// 第{0}行{1}与{2}不一致，是否{3}？。
        /// </summary>
        I_0007,
        #endregion

        #region 警告消息

        /// <summary>
        /// {0}
        /// </summary>
        W_0000,
        /// <summary>
        /// 信息尚未保存，确定进行当前操作？
        /// </summary>
        W_0001,
        /// <summary>
        /// 没有可删除的信息
        /// </summary>
        W_0002,
        /// <summary>
        /// {0}已被{1}引用，不可删除{0}！
        /// </summary>
        W_0003,
        /// <summary>
        /// 没有可导出的信息！
        /// </summary>
        W_0004,
        /// <summary>
        /// {0}已存在{1}，请先将{1}置为无效！
        /// </summary>
        W_0005,
        /// <summary>
        /// 请选择{0}进行{1}！
        /// </summary>
        W_0006,
        /// <summary>
        /// {0}已被{1}，不可{2}！
        /// </summary>
        W_0007,
        /// <summary>
        /// 请选择状态{0}的数据进行{1}！
        /// </summary>
        W_0008,
        /// <summary>
        /// {0}不存在，无法加载{1}！
        /// </summary>
        W_0009,
        /// <summary>
        /// {0}不存在，请{1}！
        /// </summary>
        W_0010,
        /// <summary>
        /// {0}已加入{1}，不可重复加入！
        /// </summary>
        W_0011,
        /// <summary>
        /// 确定删除数据？
        /// </summary>
        W_0012,
        /// <summary>
        /// 已选{0}条数据，确定删除？
        /// </summary>
        W_0013,
        /// <summary>
        /// 确认无误并审核?
        /// </summary>
        W_0014,
        /// <summary>
        /// {0}已达最大值{1}！
        /// </summary>
        W_0015,
        /// <summary>
        /// {0}信息为空，不能{1}！
        /// </summary>
        W_0016,
        /// <summary>
        /// 请勾选至少一条{0}信息进行{1}！
        /// </summary>
        W_0017,
        /// <summary>
        /// 确认无误并反审核?
        /// </summary>
        W_0018,
        /// <summary>
        /// {0}被其他{1}使用，是否确定{2}?
        /// </summary>
        W_0019,
        /// <summary>
        /// 当前明细数为：{0}，继续添加存在保存失败风险，是否需要新增单据后添加吗？
        /// </summary>
        W_0020,
        /// <summary>
        /// 请指定参数：{0}
        /// </summary>
        W_0021,
        /// <summary>
        /// 请确保数据源Model中存在属性：{0}
        /// </summary>
        W_0022,
        /// <summary>
        /// 请先{0}，再{1}
        /// </summary>
        W_0023,
        /// <summary>
        /// 没有获取到{0}，{1}失败！
        /// </summary>
        W_0024,
        /// <summary>
        /// 已选{0}条数据中包含{1}条{2}为{3}的数据，是否忽略？\r\n单击【确定】忽略并删除其他数据，【取消】返回。
        /// </summary>
        W_0025,
        /// <summary>
        /// 已有数据被使用，不能删除，单击【确定】，忽略并删除其他，点击【取消】返回。
        /// </summary>
        W_0026,
        /// <summary>
        /// 当前用户没有{1}的权限，您可以导出零库存并将盘盈的配件入库\r\n或\r\n联系管理员申请授权。
        /// </summary>
        W_0027,
        /// <summary>
        /// 当前用户没有{1}的权限，您可以联系管理员申请授权。
        /// </summary>
        W_0028,
        /// <summary>
        /// 您删除的数据中{0}是来自于采购预测单明细，是否删除？。
        /// </summary>
        W_0029,
        /// <summary>
        /// {0}，打开界面失败！
        /// </summary>
        W_0030,
        /// <summary>
        /// 历史应收单与当前应收单应收总金额为{0}\r\n销售订单中销售金额为{1}\r\n二者不相等，是否确认审核？
        /// </summary>
        W_0031,
        /// <summary>
        /// 是否确认无误并签收？
        /// </summary>
        W_0032,
        /// <summary>
        /// 这是最后一次签收，确定后将无法进行下一次签收\r\n单击【确定】签收单据，【取消】返回。
        /// </summary>
        W_0033,
        /// <summary>
        /// 请先选择{0}，再选择{1}。
        /// </summary>
        W_0034,
        /// <summary>
        /// 单据{0}的已收总金额与本次收款金额共{1}\r\n大于应收总金额{1}\r\n是否确认审核？
        /// </summary>
        W_0035,
        /// <summary>
        /// {0}的销售订单不能转收款，是否对其他单据进行收款？
        /// </summary>
        W_0036,
        /// <summary>
        /// {0}大于{1}，是否确认{2}？
        /// </summary>
        W_0037,
        /// <summary>
        /// 确认无误并核实?
        /// </summary>
        W_0038,
        /// <summary>
        /// 客户：{0}的欠款金额为{1}，超过信用额度{2}，是否确认审核？
        /// </summary>
        W_0039,
        /// <summary>
        /// 确认无误并{0}?
        /// </summary>
        W_0040,
        #endregion
    }
}
