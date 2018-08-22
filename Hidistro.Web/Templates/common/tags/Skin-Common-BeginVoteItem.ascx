<%@ Control Language="C#"  %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    <li>
        <h6><%# Eval("VoteItemName") %></h6>
        <div class="select" style="float:none;margin-left:0px;width:100%;">
            <div class="progress">
                <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="60"
                    aria-valuemin="0" aria-valuemax="100" style="width: <%# Eval("Percentage") %>%;">
                </div>
                <span class="complete check"><%# Eval("ItemCount") %>(<%# Eval("Percentage") %>%)</span>
            </div>
            <label>
                <input type="radio" name="VoteItem" value="<%# Eval("VoteItemId") %>">选择
            </label>
        </div>
    </li>

