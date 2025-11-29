# DORA Capabilities Assessment

This document outlines the capabilities that drive software delivery performance, grouped by their impact area.

## Capabilities that enable a Climate for Learning

### 1. [How does your team ensure that code remains maintainable over time?](https://dora.dev/capabilities/code-maintainability/)
1. We don’t have a defined approach
2. We use basic linting and formatting tools
3. We follow code review and documentation guidelines
4. We apply automated maintainability checks and refactoring practices
5. We have a culture of continuous refactoring and evolutionary architecture

### 2. [How does your team ensure the quality and usefulness of documentation?](https://dora.dev/capabilities/documentation-quality/)
1. We rarely document or our documentation is outdated
2. We create basic documentation for major features when needed
3. We maintain up-to-date documentation for key systems and processes
4. We have standards for documentation quality and regularly review it
5. We treat documentation as a core deliverable, with clear ownership, continuous improvement, and easy discoverability

### 3. [To what extent is your team empowered to choose the tools they use?](https://dora.dev/capabilities/teams-empowered-to-choose-tools/)
1. We have no say in tool selection and must use what is mandated
2. We can suggest tools, but adoption requires lengthy approval
3. We can choose some tools within predefined organizational guidelines
4. We are free to choose most of our tools, with coordination for compatibility
5. We have full autonomy to choose and adapt our tools to meet our needs, while sharing best practices across teams

### 4. [How would you describe your organization's culture in terms of trust, collaboration, and information sharing?](https://dora.dev/capabilities/generative-organizational-culture/)
1. Our culture is low trust, with limited collaboration and information kept siloed
2. Some teams collaborate, but information is mostly shared on a need-to-know basis
3. We have a generally cooperative culture, but information flow can be inconsistent
4. We have high trust and open communication, with regular cross-team collaboration
5. We have a high-trust, generative culture where information flows freely, collaboration is the norm, and people feel safe to share ideas and learn from mistakes

### 5. [How satisfied are you with your job and work environment?](https://dora.dev/capabilities/job-satisfaction/)
1. I am generally dissatisfied with my job and work environment
2. I experience occasional satisfaction, but issues often outweigh the positives
3. I am mostly satisfied, though there are some recurring challenges
4. I am satisfied with my job, my work environment, and the support I receive
5. I am highly satisfied and feel motivated, supported, and fulfilled in my work

### 6. [How would you describe your team's culture around learning and skill development?](https://dora.dev/capabilities/learning-culture/)
1. Learning is not prioritized and rarely happens during work hours
2. Learning happens occasionally, but mostly driven by individual initiative
3. Learning is encouraged, with some structured opportunities provided
4. We actively support continuous learning with regular time and resources dedicated
5. We have a strong learning culture where continuous improvement, knowledge sharing, and experimentation are part of everyday work

### 7. [How often does your team run experiments to improve processes, tools, or outcomes?](https://dora.dev/capabilities/team-experimentation/)
1. We rarely or never run experiments
2. We occasionally try new things, but without a structured approach
3. We experiment periodically and sometimes measure the results
4. We regularly run experiments with clear goals and evaluate outcomes
5. Experimentation is part of our culture, with frequent, data-driven trials that inform continuous improvement

### 8. [How would you describe the leadership style in your organization?](https://dora.dev/capabilities/transformational-leadership/)
1. Leaders focus mainly on tasks and short-term results, with little inspiration or vision
2. Leaders occasionally provide vision or motivation, but mostly manage day-to-day work
3. Leaders set a clear direction and provide support, but may not consistently inspire or challenge the team
4. Leaders inspire, motivate, and support growth, fostering a shared sense of purpose
5. Leaders consistently demonstrate transformational leadership—sharing a compelling vision, encouraging innovation, empowering people, and leading by example

### 9. [How well does your organization support your well-being and work-life balance?](https://dora.dev/capabilities/well-being/)
1. Well-being is not considered, and workloads are often unsustainable
2. Some attention is given to well-being, but long hours or stress are common
3. Well-being is valued, with occasional initiatives to support balance
4. Well-being is actively supported through policies, flexibility, and resources
5. Well-being is a core part of our culture, with sustainable workloads, strong support systems, and a healthy work-life balance

---

## Capabilities that enable Fast Flow

### 10. [How effectively does your team practice continuous delivery?](https://dora.dev/capabilities/continuous-delivery/)
1. We deploy infrequently, with manual processes and long lead times
2. We have some automation, but releases still require significant manual effort
3. We deploy regularly with mostly automated pipelines, though some manual steps remain
4. We have fully automated pipelines and can deploy to production on demand
5. We practice true continuous delivery, deploying small changes frequently and safely with rapid feedback loops

### 11. [How does your team manage database changes?](https://dora.dev/capabilities/database-change-management/)
1. Database changes are manual, risky, and often cause downtime
2. We have basic scripts for database changes, but testing is minimal
3. We version-control database changes and test them in non-production environments
4. We automate database change deployments with rollback strategies
5. We fully integrate automated, tested, and reversible database changes into our deployment pipeline

### 12. [How automated are your deployment processes?](https://dora.dev/capabilities/deployment-automation/)
1. Deployments are entirely manual and time-consuming
2. Some deployment steps are automated, but manual intervention is still required
3. Most deployment steps are automated, with occasional manual tasks
4. Deployments are fully automated and repeatable
5. Deployments are fully automated, fast, reliable, and can be triggered on demand with minimal risk

### 13. [How flexible and on-demand is your infrastructure provisioning?](https://dora.dev/capabilities/flexible-infrastructure/)
1. Infrastructure is fixed, slow to provision, and requires manual setup
2. Some infrastructure can be provisioned through tickets or requests, but it’s still slow
3. We use some infrastructure-as-code or scripts to provision environments
4. We have automated, self-service infrastructure provisioning through infrastructure-as-code
5. Infrastructure is fully automated, self-service, scalable on demand, and integrated into our delivery pipeline

### 14. [How independently can your team deliver and deploy changes without relying on other teams?](https://dora.dev/capabilities/loosely-coupled-teams/)
1. We are highly dependent on other teams for most changes
2. We can make some changes independently, but often require coordination with others
3. We can deliver many changes on our own, but still have some key dependencies
4. We are largely independent, with minimal dependencies on other teams
5. We are fully autonomous, able to design, build, test, and deploy changes without external dependencies

### 15. [How streamlined is your change approval process?](https://dora.dev/capabilities/streamlining-change-approval/)
1. Change approvals are slow, manual, and require multiple layers of review
2. Approvals are somewhat standardized but still require significant manual steps
3. We have clear approval guidelines and some automation in the process
4. Approvals are automated for low-risk changes and fast for others
5. We use automated, risk-based change approval with guardrails, enabling fast and safe deployments

### 16. [How closely does your team follow trunk-based development practices?](https://dora.dev/capabilities/trunk-based-development/)
1. We work on long-lived branches that are merged infrequently
2. We merge branches occasionally, but integration is often slow and causes conflicts
3. We integrate changes into the main branch at least once a week
4. We integrate changes into the main branch at least once a day
5. We commit small, frequent changes directly to the trunk or use short-lived branches merged multiple times per day

### 17. [How effectively does your team use version control for all production artifacts?](https://dora.dev/capabilities/version-control/)
1. We do not use version control consistently for our code or artifacts
2. We use version control for application code, but not for all artifacts
3. We version-control all code and some other production artifacts
4. We version-control all production artifacts, including code, configurations, and scripts
5. We version-control everything needed to recreate our environments, ensuring full reproducibility

### 18. [How effectively does your team use visual management to track work and progress?](https://dora.dev/capabilities/visual-management/)
1. We have little to no visual tracking of work
2. We use basic boards or lists, but they are often outdated or incomplete
3. We maintain visual boards that are generally up to date and visible to the team
4. We use well-maintained, visible boards that help manage flow and identify bottlenecks
5. We have highly visible, real-time visual management systems that guide decision-making, improve flow, and make work status clear to all stakeholders

### 19. [How does your team manage work in progress (WIP) limits?](https://dora.dev/capabilities/wip-limits/)
1. We do not set or track WIP limits
2. We occasionally set WIP limits, but they are not consistently followed
3. We set WIP limits for some work items and review them periodically
4. We set and consistently enforce WIP limits to maintain flow
5. We actively manage and adjust WIP limits based on team capacity and flow metrics to optimize delivery

### 20. [How consistently does your team work in small batches?](https://dora.dev/capabilities/working-in-small-batches/)
1. We typically work on large batches that take weeks or months to complete
2. We sometimes break work into smaller pieces, but large batches are still common
3. We regularly work in moderately sized batches to reduce risk
4. We consistently work in small batches that can be delivered in days
5. We always work in very small batches, enabling rapid feedback, quick delivery, and minimal risk

---

## Capabilities that enable Fast Feedback

### 21. [How effectively does your team practice continuous integration?](https://dora.dev/capabilities/continuous-integration/)
1. We integrate code infrequently, leading to large and risky merges
2. We integrate code occasionally, but with minimal automated testing
3. We integrate code at least daily, with automated builds and basic tests
4. We integrate code multiple times per day with comprehensive automated tests
5. We practice true continuous integration with frequent merges, fast automated feedback, and fixes for broken builds immediately prioritized

### 22. [How frequently and effectively does your team gather and use customer feedback?](https://dora.dev/capabilities/customer-feedback/)
1. We rarely gather customer feedback
2. We gather feedback occasionally, but it is not systematically used
3. We regularly gather feedback and sometimes use it to inform decisions
4. We gather feedback frequently and use it to guide product improvements
5. We have continuous feedback loops where customer input directly drives ongoing product and delivery decisions

### 23. [How effectively does your team monitor systems and use observability to detect and resolve issues?](https://dora.dev/capabilities/monitoring-and-observability/)
1. We have little to no monitoring, and issues are often found by customers
2. We have basic monitoring, but it only covers some systems or key metrics
3. We have monitoring for most systems and use it to detect and troubleshoot issues
4. We have comprehensive monitoring and observability, enabling rapid detection and resolution
5. We have proactive, automated observability with actionable alerts and deep insights that prevent issues before they impact customers

### 24. [How effectively does your team monitor system health and performance?](https://dora.dev/capabilities/monitoring-systems/)
1. We have little to no monitoring, and problems are usually reported by users
2. We have basic monitoring that covers only a few key systems or metrics
3. We monitor most systems and can detect many issues before they escalate
4. We have comprehensive, automated monitoring with clear alerting for critical issues
5. We have proactive monitoring across all systems, with intelligent alerts and actionable insights that help prevent incidents

### 25. [How well is security integrated into your software delivery process?](https://dora.dev/capabilities/pervasive-security/)
1. Security is handled separately and only at the end of development
2. Security reviews happen occasionally, often late in the process
3. Security is considered during development, with some automated checks
4. Security practices and tools are integrated throughout the development lifecycle
5. Security is pervasive, automated, and embedded in every stage of development, with a strong culture of shared responsibility

### 26. [How effectively are failures detected and communicated to the right people?](https://dora.dev/capabilities/proactive-failure-notification/)
1. Failures are usually found by customers or go unnoticed for long periods
2. We sometimes detect failures internally, but alerts are inconsistent or delayed
3. We detect most failures internally and notify relevant people in a timely manner
4. We proactively detect and communicate failures quickly to the right stakeholders
5. We have automated, proactive failure detection and instant, targeted notifications that enable rapid response before customers are impacted

### 27. [How effectively does your team use automated testing?](https://dora.dev/capabilities/test-automation/)
1. We rely almost entirely on manual testing
2. We have some automated tests, but coverage is low and maintenance is inconsistent
3. We have a good level of automated test coverage for critical functionality
4. We have comprehensive automated tests across multiple levels (unit, integration, end-to-end)
5. We have extensive, reliable, and fast automated tests that run continuously and provide rapid feedback to prevent defects

### 28. [How does your team manage test data for automated and manual testing?](https://dora.dev/capabilities/test-data-management/)
1. Test data is created manually and inconsistently, often slowing down testing
2. We have some shared test data sets, but they are hard to maintain or reuse
3. We maintain version-controlled test data for key scenarios
4. We have automated processes for creating, managing, and refreshing test data
5. We manage test data automatically, with self-service, on-demand generation and anonymization that supports fast, reliable, and compliant testing

### 29. [How visible is work across your entire value stream, from idea to delivery?](https://dora.dev/capabilities/work-visibility-in-value-stream/)
1. Work is largely invisible beyond individual teams or functions
2. Some work is tracked across stages, but visibility is fragmented
3. We have tools or dashboards showing work across most of the value stream
4. We have clear, real-time visibility into work across the entire value stream
5. We have full, real-time transparency across the value stream, enabling data-driven decisions and continuous improvement