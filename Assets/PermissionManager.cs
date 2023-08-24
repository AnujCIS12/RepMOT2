using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PermissionManager : SingletonMonoBehaviour<PermissionManager>
{
    Dictionary<string, NetworkConst.role1> rolesDic = new Dictionary<string, NetworkConst.role1>();
    
    public bool GetRolePermission(string role)
    {
        foreach(KeyValuePair<string, NetworkConst.role1> tempRole in rolesDic)
        {
            if (tempRole.Key == role) { 
                return true;
            }
            
        }
        return false;
    }

    public bool GetPermissionForRole(string role, string permission)
    {
        //if (!IsContainsModule(role)) return false;
        NetworkConst.role1 tempRole;
        if (rolesDic.TryGetValue(role, out tempRole))
        {
            foreach (NetworkConst.permission tempPermission in tempRole.permissions)
            {
                if (tempPermission.name == permission)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool GetPermissionForRole1(string role, string permission)
    {
        //if (!IsContainsModule(role)) return false;
        Debug.Log("Size : "+rolesDic.Count);
        foreach (KeyValuePair<string, NetworkConst.role1> tempRole in rolesDic)
        {
            Debug.Log("----Key------" + tempRole.Key);
            //Assert.IsTrue(tempRole.Value.name.CaseInsensitiveContains(role));
            //if (tempRole.Value.name.Contains(role))
            if (tempRole.Value.name.CaseInsensitiveContains("Admin")) return true;//return true if admin role is assigned
            if (tempRole.Value.name.CaseInsensitiveContains(role))
            {
                foreach (NetworkConst.permission tempPermission in tempRole.Value.permissions)
                {
                    if (tempPermission.name.Equals(permission, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }
    public bool IsContainsModule(string moduleName)
    {
        foreach (KeyValuePair<string, NetworkConst.role1> tempRole in rolesDic)
        {
            
            if (tempRole.Value.name.Contains(moduleName))
            {
                return true;
            }

        }
        return false;
    }

    public void loadPermissions()
    {
        NetworkConst.allRolesRes1 _allRolesRes1;
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 4;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAssignedRolesData(json, (string data) =>
        {
            _allRolesRes1 = JsonUtility.FromJson<NetworkConst.allRolesRes1>(data);
            
                Debug.Log("---------Data -------------" + _allRolesRes1);
                rolesDic.Clear();
                foreach (NetworkConst.role1 tmpRole1 in _allRolesRes1.data)
                {
                    Debug.Log(tmpRole1.name + "..................Name adding");
                    if (!rolesDic.ContainsKey(tmpRole1.name))
                        rolesDic.Add(tmpRole1.name, tmpRole1);
                }

        });
    }

    public string GetRoleNames()
    {
        string name;
        name = "";
        foreach (KeyValuePair<string, NetworkConst.role1> tempRole in rolesDic)
        {
            name += tempRole.Key+", ";
        }

        return name;
    }
}


public static class Extensions
{
    public static bool CaseInsensitiveContains(this string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    {
        return text.IndexOf(value, stringComparison) >= 0;
    }
}

