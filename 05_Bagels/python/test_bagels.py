from bagels import build_result_string


def test_build_result_string() -> None:
    build_result_string(["a", "b", "c"], "abc")
