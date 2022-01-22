Imports Xunit
Imports Animal
Imports System.IO

Public Class TestContainer
    Private Shared Function ResponseVariantExpander(src As IEnumerable(Of String)) As TheoryData(Of String)
        Dim theoryData = New TheoryData(Of String)
        src.
            SelectMany(Function(x) {x, x.Substring(0, 1)}).
            SelectMany(Function(x) {
                x,
                x.ToUpperInvariant,
                x.ToLowerInvariant,
                x.ToTitleCase,
                x.ToReverseCase
            }).
            Distinct.
            ForEach(Sub(x) theoryData.Add(x))
        Return theoryData
    End Function
    Private Shared YesVariantsThepryData As TheoryData(Of String) = ResponseVariantExpander({"yes", "true", "1"})
    Private Shared Function YesVariants() As TheoryData(Of String)
        Return YesVariantsThepryData
    End Function
    Private Shared NoVariantsThepryData As TheoryData(Of String) = ResponseVariantExpander({"no", "false", "0"})
    Private Shared Function NoVariants() As TheoryData(Of String)
        Return NoVariantsThepryData
    End Function

    ''' <summary>Test LIST variants</summary>
    <Theory>
    <InlineData("LIST")>
    <InlineData("list")>
    <InlineData("List")>
    <InlineData("lIST")>
    Sub List(listResponse As String)
        Dim console As New MockConsole({listResponse})
        Dim game As New Game(console)
        Assert.Throws(Of EndOfInputsException)(Sub() game.BeginLoop())
        Assert.Equal(
            {
                "ANIMALS I ALREADY KNOW ARE:",
                "FISH           BIRD           "
            },
            console.Lines.Slice(-4, -2).Select(Function(x) x.line)
        )
    End Sub

    '' <summary>Test YES variants</summary>
    <Theory>
    <MemberData(NameOf(YesVariants))>
    Sub YesVariant(yesVariant As String)
        Dim console As New MockConsole({yesVariant})
        Dim game As New Game(console)
        Assert.Throws(Of EndOfInputsException)(Sub() game.BeginLoop())
        Assert.Equal(
            {
                $"ARE YOU THINKING OF AN ANIMAL? {yesVariant}",
                "DOES IT SWIM? "
            },
            console.Lines.Slice(-2, 0).Select(Function(x) x.line)
        )
    End Sub

    '' <summary>Test NO variants</summary>
    <Theory>
    <MemberData(NameOf(NoVariants))>
    Sub NoVariant(noVariant As String)
        Dim console As New MockConsole({"y", noVariant})
        Dim game As New Game(console)
        Assert.Throws(Of EndOfInputsException)(Sub() game.BeginLoop())
        Assert.Equal(
            {
                $"DOES IT SWIM? {noVariant}",
                "IS IT A BIRD? "
            },
            console.Lines.Slice(-2, 0).Select(Function(x) x.line)
        )
    End Sub

    ''' <summary>Test adding a new animal and using the new animal in the game</summary>
    <Fact>
    Sub TestAddedAnimal()
        Dim console As New MockConsole({
            "y",
            "y",
            "n",
            "whale",
            "is it a mammal?",
            "y",
            "y",
            "y",
            "y",
            "y"
        })
        Dim game As New Game(console)
        Assert.Throws(Of EndOfInputsException)(Sub() game.BeginLoop())
        Assert.Equal(
            {
                "ARE YOU THINKING OF AN ANIMAL? y",
                "DOES IT SWIM? y",
                "IS IT A FISH? n",
                "THE ANIMAL YOU WERE THINKING OF WAS A ? whale",
                "PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A",
                "WHALE FROM A FISH",
                "is it a mammal?",
                "FOR A WHALE THE ANSWER WOULD BE? y",
                "ARE YOU THINKING OF AN ANIMAL? y",
                "DOES IT SWIM? y",
                "IS IT A MAMMAL? y",
                "IS IT A WHALE? y",
                "WHY NOT TRY ANOTHER ANIMAL?",
                "ARE YOU THINKING OF AN ANIMAL? "
            },
            console.Lines.Slice(9, 100).Select(Function(x) x.line)
        )
    End Sub
End Class
