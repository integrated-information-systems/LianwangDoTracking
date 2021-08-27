Imports System.Drawing

Partial Class ManageUsers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        BindUsers()
        'End If
    End Sub
    Protected Sub BindUsers()
        UsersList.DataSource = Membership.GetAllUsers
        UsersList.DataBind()
    End Sub
    Protected Sub UsersList_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim UserName As String = UsersList.Rows(e.RowIndex).Cells(1).Text

        If UserName.ToLower = "admin" Then
            lblMsg.Text = String.Format("You Cannot Delete {0} User", UserName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
        Else
            Dim SelectedUser As MembershipUser = Membership.GetUser(UserName)
            If Not IsNothing(SelectedUser) Then
                SelectedUser.IsApproved = False
                Membership.UpdateUser(SelectedUser)
            End If
            lblMsg.Text = String.Format("User ""{0}""  deleted Successfully", UserName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
            BindUsers()
        End If
    End Sub

    Protected Sub UsersList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UsersList.RowDataBound
        If e.Row.RowIndex > -1 Then
            Dim MUser As MembershipUser = Membership.GetUser(e.Row.Cells(1).Text)
            If MUser.IsApproved = False Then
                'If MUser.IsApproved = False And MUser.UserName = "test" Then
                'Membership.DeleteUser(Membership.DeleteUser(MUser.UserName))
                'e.Row.Visible = False
                e.Row.BackColor = Color.Red
                e.Row.ForeColor = Color.White
                e.Row.Cells(0).Text = String.Empty
                'MUser.IsApproved = True
                'Membership.UpdateUser(MUser)
                else
                Dim lb As LinkButton = e.Row.Cells(0).Controls(0)
                lb.OnClientClick = "return confirm('Are you certain you want to delete?');"
            End If
            'Dim lb As LinkButton = e.Row.Cells(0).Controls(0)
            'lb.OnClientClick = "return confirm('Are you certain you want to delete?');"

            'Response.Write(lb.Text)
            'Response.Write(e.Row.Cells(0).Controls(.Count)
        End If
    End Sub

    Protected Sub UsersList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UsersList.SelectedIndexChanged
        lblMsg.Visible = False
        lblMsg.Text = ""
        
        UserName.Text = UsersList.Rows(UsersList.SelectedIndex).Cells(1).Text
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
       
        Email.Text = MUser.Email


    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
        If Password.Text <> String.Empty Then
            MUser.UnlockUser()
            MUser.ChangePassword(MUser.ResetPassword(), Password.Text)
        End If


        MUser.Email = Email.Text

        Membership.UpdateUser(MUser)
        Clear()
        lblMsg.Visible = True
        lblMsg.Text = "Updated Successfully"
        lblMsg.ForeColor = Color.Green
        BindUsers()
    End Sub
    Private Sub Clear()
        UsersList.SelectedIndex = -1
        UserName.Text = String.Empty
        Email.Text = String.Empty
        Password.Text = String.Empty
        ConfirmPassword.Text = String.Empty

    End Sub
End Class
