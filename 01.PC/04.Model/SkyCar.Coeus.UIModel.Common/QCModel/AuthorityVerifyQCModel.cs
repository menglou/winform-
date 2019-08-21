namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 菜单动作授权验证QCModel
    /// </summary>
    public class AuthorityVerifyQCModel
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string MenuId { get; set; }
        public string ActionKey { get; set; }
        public string MenuFullName { get; set; }
    }
}
