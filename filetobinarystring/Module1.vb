Imports System.IO
Module Module1
    Sub Main()
        Console.WriteLine(CreateBinarySeed("\\internal.hereford.ac.uk\student\2018 enrolment\ntr8346\My Documents\Database1.accdb"))
        Console.ReadKey()
    End Sub
    Public Function ByteToBin(ByVal conv() As Byte) As String
        Dim newBin As New System.Text.StringBuilder
        For Each c In conv
            newBin.Append(Convert.ToString(c, 2).PadLeft(8, "0"))
        Next
        Return newBin.ToString
    End Function
    Public Function CreateBinarySeed(Location As String)
        Dim data() As Byte = File.ReadAllBytes(Location)
        Dim binary As String = ByteToBin(data)
        Dim binaryint_s As String = ""
        Dim StepVariable As Integer = 64
        Dim PercentComplete, PreviousPercent As Integer
        PreviousPercent = 0
        'from starting value of the string to the end with a step of 64 until there are less than 64 chars to read, 
        'then step variable changes to that value of chars so that full string is read.
        For i = 0 To binary.Length - 1 Step StepVariable
            'if there isnt 64 chars left, it jumps to the else:
            If (i + 64) <= binary.Length Then
                'If binary Then Integer value Is negative, inverts value To positive, And appends 'N' to the start to signify its negative, 
                'allows String.Split("-") function to work as having the negative would cause a null array value.
                If Convert.ToInt64((binary.Substring(i, 64)), 2) < 0 Then
                    binaryint_s = binaryint_s & "-" & "N" & CStr(0 - (Convert.ToInt64((binary.Substring(i, 64)), 2)))
                Else
                    binaryint_s = binaryint_s & "-" & CStr(Convert.ToInt64((binary.Substring(i, 64)), 2))
                End If
                'Else statement appends the ammount of binary digits read at the end to give the final answer only happens if last set is less than 64.
            Else
                'B signifies when the value states how many bits are read at the end.
                binaryint_s = binaryint_s & "-" & CStr(Convert.ToInt64((binary.Substring(i, binary.Length - i)), 2)) & "B" & binary.Length - i
            End If
            If i + 64 > binary.Length Then
                StepVariable = binary.Length - i
            End If
            PercentComplete = Math.Round((i / binary.Length) * 100)
            If PercentComplete <> PreviousPercent Then
                Console.Clear()
                Console.WriteLine(PercentComplete & "% complete")
                PreviousPercent = Math.Round((i / binary.Length) * 100)
            End If
        Next
        Return (binaryint_s)
    End Function
End Module
