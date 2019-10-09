Imports System.IO
Module Module1

    Sub Main()
        AdminControl()
    End Sub
    Dim array() As String
    Public Function ByteToBin(ByVal conv() As Byte) As String
        Dim newBin As New System.Text.StringBuilder
        'Parallel.For(0, conv.Length, myOptions, Sub(c) newBin.Append(Convert.ToString(Convert.ToInt64(conv(c)), 2).PadLeft(8, "0")))
        For Each c In conv
            newBin.Append(Convert.ToString(c, 2).PadLeft(8, "0"))
        Next
        Return newBin.ToString
    End Function
    Public Function CreateBinarySeed(ByRef Location As String)
        Dim data() As Byte = File.ReadAllBytes(Location)
        Dim binary As String = ByteToBin(data)
        Dim Settings As ParallelOptions = New ParallelOptions With {.MaxDegreeOfParallelism = Environment.ProcessorCount}
        Dim length As Integer = Math.Ceiling(binary.Length / 64)
        ReDim array(length)
        Parallel.For(0, CInt(Math.Ceiling((binary.Length - 1) / 64)), Settings, Sub(i) Loops(i, binary))
        Return (String.Join("", array))
    End Function
    Sub WriteSeed(ByRef Seed As String)
        File.WriteAllText("C:\Users\minec\Desktop\seed.txt", Seed)
    End Sub
    Sub Loops(ByRef i As Integer, ByRef binary As String)
        'if there isnt 64 chars left, it jumps to the else:
        If (i * 64 + 64) <= binary.Length Then
            'If binary Then Integer value Is negative, inverts value To positive, And appends 'N' to the start to signify its negative, 
            'allows String.Split("-") function to work as having the negative would cause a null array value.
            If Convert.ToInt64((binary.Substring(i, 64)), 2) < 0 Then
                array(i) = "-" & "N" & CStr(Math.Abs(Convert.ToInt64((binary.Substring(i * 64, 64)), 2)))
            Else
                array(i) = "-" & CStr(Convert.ToInt64((binary.Substring(i * 64, 64)), 2))
            End If
            'Else statement appends the ammount of binary digits read at the end to give the final answer only happens if last set is less than 64.
        Else
            'B signifies when the value states how many bits are read at the end.
            array(i) = "-" & CStr(Convert.ToInt64((binary.Substring(i, binary.Length - i * 64)), 2)) & "B" & binary.Length - i * 64
        End If
        Console.WriteLine(i)

    End Sub
    Sub AdminControl()
        Dim input As String = Console.ReadLine()
        If input = "a" Then
            Console.WriteLine(CreateBinarySeed("C:\Users\minec\Desktop\1mbdata.txt"))
            Console.ReadKey()
        ElseIf input = "b" Then
            WriteSeed(CreateBinarySeed("C:\Users\minec\Desktop\1mbdata.txt"))
            Console.Write("written")
            Console.ReadKey()
        ElseIf input = "c" Then
            Dim data() As Byte = File.ReadAllBytes("C:\Users\minec\Desktop\test.txt")
            Console.WriteLine(ByteToBin(data))
            Console.ReadLine()
        End If
    End Sub

End Module
