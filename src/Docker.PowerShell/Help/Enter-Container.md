---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Enter-Container
## SYNOPSIS
Enter-Container \[-Id\] \<string\> \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Enter-Container \[-Container\] \<ContainerListResponse\> \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default
```
Enter-Container [-Id] <String> [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Enter-Container [-Container] <ContainerListResponse> [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION

Connects to interactive session in the specified container.
## EXAMPLES

### Example 1
```
PS C:\> {{ Enter-Container $myContainer}}
```

Connects to $myContainer
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named “key.pfx” that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).





```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Container
Specifies the container to connect to.





```yaml
Type: ContainerListResponse
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -HostAddress
The address of the docker daemon to connect to.





```yaml
Type: String
Parameter Sets: Default
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
Specifies the container by id or name. You can pass a subset of the ID if it is unique.





```yaml
Type: String
Parameter Sets: Default
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String
Docker.DotNet.Models.ContainerListResponse
## OUTPUTS

### System.Object

## NOTES
These are some notes about the cmdlet. 

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell)






