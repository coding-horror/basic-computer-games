package com.pcholt.console.testutils

import com.google.common.truth.Truth
import org.junit.Rule
import org.junit.contrib.java.lang.system.SystemOutRule
import org.junit.contrib.java.lang.system.TextFromStandardInputStream

abstract class ConsoleTest {
    @get:Rule
    val inputRule = TextFromStandardInputStream.emptyStandardInputStream()

    @get:Rule
    val systemOutRule = SystemOutRule().enableLog()

    val regexInputCommand = "\\{(.*)}".toRegex()

    fun assertConversation(conversation: String, runMain: () -> Unit) {

        inputRule.provideLines(*regexInputCommand
            .findAll(conversation)
            .map { it.groupValues[1] }
            .toList().toTypedArray())

        runMain()

        Truth.assertThat(
            systemOutRule.log.trimWhiteSpace()
        )
            .isEqualTo(
                regexInputCommand
                    .replace(conversation, "").trimWhiteSpace()
            )
    }

    private fun String.trimWhiteSpace() =
        replace("[\\s]+".toRegex(), " ")
}